using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment_assignment.DataLayer;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Payment_assignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ValidateModel]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
private PaymentDB _PaymentDB;
        public HomeController(ILogger<HomeController> logger,PaymentDB _PaymentDBContext)
        {
            this._logger = logger;
            this._PaymentDB = _PaymentDBContext;
        }
        
     [HttpPost]
     public string Post(payments _payment ){
         
        using (var context = this._PaymentDB)
        {
            string completed = "Completed";
            string fail = "fail";

            _payment.Status = "Pending";
            var std = context.payments.Add(_payment); 
            context.SaveChanges();
            if(_payment.Amount < 20){
              
              _payment.Status = ((ICheapPaymentGateway) _payment).pay(_payment) ? completed : fail;  

            }else if(_payment.Amount <= 500 )
            {
                if( !((IExpensivePaymentGateway) _payment).pay(_payment)){
             _payment.Status = ((ICheapPaymentGateway) _payment).pay(_payment) ? completed : fail;
               
              
               }else{
                   _payment.Status = completed;
               }
            }else {
                IPremiumPaymentService _pay = _payment;
                int counter = 1;
                while(counter < 4){

                if(_pay.pay(_payment)){
                    _payment.Status = completed;
                    break;
                }else {
                    counter++;
                    if(counter == 3)
                    _payment.Status = fail;
                }
            }
        }
    
    // No need for eager loading

            // var _UpdateStatus = (from p in  context.payments
            //                     where  p.Id == _payment.Id
            //                     select p
            //                     ).FirstOrDefault<payments>();
            // context.Update(_UpdateStatus);
            context.SaveChanges();
        return _payment.Status;
     } 
 
}
   public class ValidateModel :  ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
}
}
