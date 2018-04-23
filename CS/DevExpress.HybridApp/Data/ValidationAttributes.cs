using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

//_ This demo targets .NET Framework 4.0. Many validation attributes that exist in .NET Framework 4.5 cannot be used.
//_ That is why we have created our own analogs of these attributes for this demo.
//_ If your application targets .NET Framework 4.5, use default validation attributes.
//_ If your application targets .NET Framework 4.0 you can copy and use these attributes or use DevExpress Validation Fluent API instead.

namespace DevExpress.DataAnnotations {
    public abstract class RegexAttributeBase : DataTypeAttribute {
#if !SL
        protected const RegexOptions DefaultRegexOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase;
#else
        protected const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase;
#endif

        readonly Regex regex;

        public RegexAttributeBase(string regex, string defaultErrorMessage, DataType dataType)
            : this(new Regex(regex, DefaultRegexOptions), defaultErrorMessage, dataType) {
        }
        public RegexAttributeBase(Regex regex, string defaultErrorMessage, DataType dataType)
            : base(dataType) {
            this.regex = (Regex)regex;
            this.ErrorMessage = defaultErrorMessage;
        }
#if !SL
        public sealed override bool IsValid(object value) {
#else
        bool IsValid(object value) {
#endif
            if(value == null)
                return true;
            string input = value as string;
            return input != null && regex.Match(input).Length > 0;
        }
#if SL
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            return IsValid(value) ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.MemberName));
        }
#endif
    }
    public sealed class PhoneAttribute : RegexAttributeBase {
        static readonly Regex regex = new Regex(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", DefaultRegexOptions);
        const string Message = "The {0} field is not a valid phone number.";
        public PhoneAttribute()
            : base(regex, Message, DataType.PhoneNumber) {
        }
    }
    public sealed class EmailAddressAttribute : RegexAttributeBase {
        static readonly Regex regex = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", DefaultRegexOptions);
        const string Message = "The {0} field is not a valid e-mail address.";
        public EmailAddressAttribute()
            : base(regex, Message, DataType.EmailAddress) {
        }
    }
    public sealed class UrlAttribute : RegexAttributeBase {
        static Regex regex = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", DefaultRegexOptions);
        const string Message = "The {0} field is not a valid fully-qualified http, https, or ftp URL.";
        public UrlAttribute()
            : base(regex, Message, DataType.Url) {
        }
    }
    public sealed class ZipCodeAttribute : RegexAttributeBase {
        static Regex regex = new Regex(@"^[0-9][0-9][0-9][0-9][0-9]$", DefaultRegexOptions);
        const string Message = "The {0} field is not a valid ZIP code.";
        public ZipCodeAttribute()
            : base(regex, Message, DataType.Url) {
        }
    }
    public sealed class CreditCardAttribute : DataTypeAttribute {
        const string Message = "The {0} field is not a valid credit card number.";
        public CreditCardAttribute()
            : base(DataType.Custom) {
            this.ErrorMessage = Message;
        }
#if !SL
        public override bool IsValid(object value) {
#else
        bool IsValid(object value) {
#endif
            if(value == null)
                return true;
            string stringValue = value as string;
            if(stringValue == null)
                return false;
            stringValue = stringValue.Replace("-", "").Replace(" ", "");
            int number = 0;
            bool oddEvenFlag = false;
            foreach(char ch in stringValue.Reverse()) {
                if(ch < '0' || ch > '9')
                    return false;
                int digitValue = (ch - '0') * (oddEvenFlag ? 2 : 1);
                oddEvenFlag = !oddEvenFlag;
                while(digitValue > 0) {
                    number += digitValue % 10;
                    digitValue = digitValue / 10;
                }
            }
            return (number % 10) == 0;
        }
#if SL
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            return IsValid(value) ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.MemberName));
        }
#endif
    }
}