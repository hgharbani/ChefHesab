using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using Microsoft.AspNetCore.Mvc;
using static ChefHesab.Share.model.ChefError;

namespace ChefHesab.Share.Extiontions
{

    public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        public Guid currentUser { get { return new Guid("FC769A7E-6A78-42CE-B7F9-0E1619CD5EFB"); } }

        protected void LogException(Exception exc)
        {
            throw new NotImplementedException("LogException");
        }

        protected virtual void SuccessNotification(string message, string title = "پیغام سیستم", bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, title, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(string message, string title = "پیغام سیستم", bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, title, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotifications(List<string> messageList, bool persistForTheNextRequest = true, bool splitMessages = false)
        {
            if (splitMessages)
            {
                foreach (string message in messageList)
                {
                    ErrorNotification(message, "پیغام سیستم", persistForTheNextRequest);
                }

                return;
            }

            string text = "";
            foreach (string message2 in messageList)
            {
                text = ((!(message2 == messageList.Last())) ? (text + message2 + "<br />") : (text + message2));
            }

            ErrorNotification(text, "پیغام سیستم", persistForTheNextRequest);
        }

        protected virtual void ErrorNotifications(List<ChefError> ChefErrors, bool persistForTheNextRequest = true)
        {
            foreach (ChefError ChefError in ChefErrors)
            {
                AddNotification(NotifyType.Error, ChefError.Title, ChefError.Message, persistForTheNextRequest);
            }
        }

        protected virtual void AddNotifications(List<ChefError> ChefErrors, bool persistForTheNextRequest = true)
        {
            List<ChefError> list = ChefErrors.Where((ChefError a) => a.ErrorTypes == ErrorType.Error || a.ErrorTypes == ErrorType.NotValid).ToList();
            if (list.Count > 0)
            {
                string text = string.Empty;
                foreach (ChefError item in list)
                {
                    text = ((list.Last() != item) ? (text + item.Message + "<br />") : (text + item));
                }

                ErrorNotification(text, "پیغام سیستم", persistForTheNextRequest);
            }

            List<ChefError> list2 = ChefErrors.Where((ChefError a) => a.ErrorTypes == ErrorType.Error).ToList();
            if (list2.Count <= 0)
            {
                return;
            }

            string text2 = string.Empty;
            foreach (ChefError item2 in list2)
            {
                text2 = ((list2.Last() != item2) ? (text2 + item2.Message + "<br />") : (text2 + item2));
            }

            WarningNotification(text2, "پیغام سیستم", persistForTheNextRequest);
        }

        protected virtual void WarningNotification(string message, string title = "پیغام سیستم", bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, title, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
            {
                LogException(exception);
            }

            ErrorNotification(exception.Message, "پیغام سیستم", persistForTheNextRequest);
        }

        protected virtual void AddNotification(NotifyType notifyType, string title, string message, bool persistForTheNextRequest)
        {
            AddNotification(new Notification
            {
                Title = title,
                Message = message,
                NotifyType = notifyType
            }, persistForTheNextRequest);
        }

        protected virtual void AddNotification(Notification notification, bool persistForTheNextRequest)
        {
            if (persistForTheNextRequest)
            {
                if (base.TempData["Chef.notifications"] == null)
                {
                    base.TempData["Chef.notifications"] = new List<Notification>();
                }

                ((List<Notification>)base.TempData["Chef.notifications"]).Add(notification);
            }
            else
            {
                if (base.ViewData["Chef.notifications"] == null)
                {
                    base.ViewData["Chef.notifications"] = new List<Notification>();
                }

                ((List<Notification>)base.ViewData["Chef.notifications"]).Add(notification);
            }
        }



        protected virtual IActionResult InvokeNotifications(bool close = false, object returnValue = null)
        {
            ChefJsonResult ChefJsonResult = new ChefJsonResult
            {
                ChefActionStaus = ChefActionStaus.Notification
            };
            if (base.TempData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.TempData["Chef.notifications"]);
            }

            if (base.ViewData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.ViewData["Chef.notifications"]);
            }

            ChefJsonResult.CloseWindows = close;
            ChefJsonResult.ReturnValue = returnValue;
            return Json(ChefJsonResult);
        }

        protected virtual IActionResult InvokeErrorNotificationsForDataSourceResult(bool close = false)
        {
            ChefJsonResult ChefJsonResult = new ChefJsonResult
            {
                ChefActionStaus = ChefActionStaus.Notification
            };
            if (base.TempData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.TempData["Chef.notifications"]);
            }

            if (base.ViewData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.ViewData["Chef.notifications"]);
            }

            ChefJsonResult.CloseWindows = close;
            DataSourceResult data = new DataSourceResult
            {
                Errors = ChefJsonResult
            };
            return Json(data);
        }

        protected virtual IActionResult InvokeNotificationsForRedirect(string redirectUrl, int redirectTimeout = 1000)
        {
            ChefJsonResult ChefJsonResult = new ChefJsonResult
            {
                ChefActionStaus = ChefActionStaus.Notification
            };
            if (base.TempData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.TempData["Chef.notifications"]);
            }

            if (base.ViewData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.ViewData["Chef.notifications"]);
            }

            ChefJsonResult.CloseWindows = false;
            ChefJsonResult.HasRedirect = true;
            ChefJsonResult.RedirectOptions.RedirectUrl = redirectUrl;
            ChefJsonResult.RedirectOptions.RedirectTimeout = redirectTimeout;
            return Json(ChefJsonResult);
        }

        protected virtual IActionResult InvokeNotificationsForConfirm(string confirmMessage, string confirmUrl)
        {
            ChefJsonResult ChefJsonResult = new ChefJsonResult
            {
                ChefActionStaus = ChefActionStaus.Confirm
            };
            if (base.TempData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.TempData["Chef.notifications"]);
            }

            if (base.ViewData["Chef.notifications"] != null)
            {
                ChefJsonResult.Notifications.AddRange((IList<Notification>)base.ViewData["Chef.notifications"]);
            }

            ChefJsonResult.CloseWindows = false;
            ChefJsonResult.HasRedirect = false;
            ChefJsonResult.ConfirmOptions.ConfirmMessage = confirmMessage;
            ChefJsonResult.ConfirmOptions.ConfirmUrl = confirmUrl;
            return Json(ChefJsonResult);
        }
    }
}
