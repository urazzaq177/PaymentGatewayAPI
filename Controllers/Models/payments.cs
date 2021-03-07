using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payment_assignment.DataLayer
{
     public class payments : ICheapPaymentGateway,IExpensivePaymentGateway,IPremiumPaymentService
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
public int Id { get; set; }
// CreditCardNumber (mandatory, string, it should be a valid CCN)
[Required]
[CreditCard]
public string CreditCardNumber { get; set; }
[Required]
public string CardHolder { get; set; }
// - ExpirationDate (mandatory, DateTime, it cannot be in the past)
[Required]
[CustomValidation(typeof(CreditCardValidation), "isFutureDate")]
public string ExpirationDate { get; set; }
// - SecurityCode (optional, string, 3 digits)
[CustomValidation(typeof(CreditCardValidation), "threeDigit")]
public string SecurityCode { get; set; }
// - Amount (mandatoy decimal, positive amount)
[Required]
[DataType("decimal(16 ,3)")]
[CustomValidation(typeof(CreditCardValidation), "isPositive")]
public decimal Amount { get; set; }
public string Status { get; set; }
    }
   
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CreditCardValidation : ValidationAttribute
{
    public static ValidationResult isFutureDate(string _dateTime)
    {
        string[] _data = _dateTime.Split("/");
        string month =  DateTime.Now.Month.ToString("D2");
        string year =DateTime.Now.Year.ToString(); 
        if(_data == null || _data.Length >2){
            return new ValidationResult("Not a valid date.");
        }else
        if (Convert.ToInt32(year + month) <= Convert.ToInt32((Convert.ToInt32(_data[1]) + 2000) + Convert.ToInt32(_data[0]).ToString("D2")))
            return ValidationResult.Success;
        else
            return new ValidationResult("Card is Expired." + year + month +  " " + (Convert.ToInt32(_data[1]) + 2000) + _data[0]);
    }
    public static ValidationResult threeDigit(int _digits){
        return _digits > 999 || _digits < 100 ?  new ValidationResult("Not a valid 3 digits code.") :  ValidationResult.Success;
    }
    public static ValidationResult isPositive(int amount){
        return amount <=0 ?  new ValidationResult("Amount is not positive") :  ValidationResult.Success;
    }
}
public interface ICheapPaymentGateway{
     bool pay(payments _payments){
        return true;
    }
}
public interface IExpensivePaymentGateway{
    bool pay(payments _payments){
        return true;
    }
}
public interface IPremiumPaymentService{
    bool pay(payments _payments){
        return true;
    }
}
}
