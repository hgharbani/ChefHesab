
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.model
{
    public class ChefJsonResult
    {
        public ChefActionStaus ChefActionStaus { get; set; }

        public List<Notification> Notifications { get; set; }

        public bool CloseWindows { get; set; }

        public object ReturnValue { get; set; }

        public bool HasRedirect { get; set; }

        public RedirectOptions RedirectOptions { get; set; }

        public ConfirmOptions ConfirmOptions { get; set; }

        public List<ChefValidationError> ValidationErrors { get; set; }

        public ChefJsonResult()
        {
            Notifications = new List<Notification>();
            HasRedirect = false;
            RedirectOptions = new RedirectOptions();
            ConfirmOptions = new ConfirmOptions();
            ValidationErrors = new List<ChefValidationError>();
        }
    }
    public class ChefValidationError
    {
        public string Key { get; set; }

        public List<string> Errors { get; set; }
    }

    public class ConfirmOptions
    {
        public string ConfirmMessage { get; set; }

        public string ConfirmUrl { get; set; }
    }
    public class RedirectOptions
    {
        public string RedirectUrl { get; set; }

        public int RedirectTimeout { get; set; }
    }
    public enum NotifyType
    {
        Success,
        Warning,
        Info,
        Error
    }
    public class Notification
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public NotifyType NotifyType { get; set; }
    }

    public enum ChefActionStaus
    {
        Notification,
        Validation,
        Confirm
    }

    public class ChefError
    {
        //
        // Summary:
        //     title of message
        public string Title { get; set; }

        //
        // Summary:
        //     message string
        public string Message { get; set; }

        public ErrorType ErrorTypes { get; set; }

        //
        // Parameters:
        //   title:
        //
        //   message:
        //
        //   errorType:
        public ChefError(string title, string message, ErrorType errorType)
        {
            Title = title;
            Message = message;
            ErrorTypes = errorType;
        }

        public enum ErrorType
        {
            [Description("خطا")]
            Error = 0,
            [Description("اخطار")]
            Warning = 0,
            [Description("خارج از اعتبار")]
            NotValid = 1,
            [Description("خطای Workflow")]
            WorkflowError = 2
        }
    }
}
