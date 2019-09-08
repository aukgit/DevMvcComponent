#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevMvcComponent.DataTypeFormat;
using DevMvcComponent.Enums;

#endregion

namespace DevMvcComponent.Error
{
    /// <summary>
    /// </summary>
    public class ErrorCollector : IDisposable
    {
        /// <summary>
        ///     High risk error css class.
        /// </summary>
        public static string HighRiskCssClass = "label label-danger high-priority";

        /// <summary>
        ///     Medium risk error css class.
        /// </summary>
        public static string MidRiskCssClass = "label label-danger medium-priority";

        /// <summary>
        ///     Low risk error css class.
        /// </summary>
        public static string LowRiskCssClass = "label label-warning low-priority";

        /// <summary>
        ///     Solution message css class.
        /// </summary>
        public static string SolutionStateCssClass = "label label-success";

        /// <summary>
        ///     Solution link css class.
        /// </summary>
        public static string SolutionStateLinkCssClass = "label label-info error-solution-solutionLink-color";

        private int _defaultCapacity = 20;
        private List<BasicError> _errors;
        private short _orderIncrementer;

        /// <summary>
        /// </summary>
        /// <param name="def"></param>
        public ErrorCollector(int def = 20)
        {
            _errors          = new List<BasicError>(def);
            _defaultCapacity = def;
        }

        public void Dispose()
        {
            _errors = null;
            GC.Collect();
        }

        /// <summary>
        ///     Get cssClass class names
        /// </summary>
        /// <param name="e"></param>
        /// <returns>
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </returns>
        public string GetCssClassForError(BasicError e)
        {
            if (!string.IsNullOrEmpty(e.CssClass))
            {
                return e.CssClass;
            }

            if (e.Type == ErrorType.High)
            {
                return HighRiskCssClass;
            }

            if (e.Type == ErrorType.Medium)
            {
                return MidRiskCssClass;
            }

            if (e.Type == ErrorType.Low)
            {
                return LowRiskCssClass;
            }

            return LowRiskCssClass;
        }

        /// <summary>
        ///     Is any error exist in the list?
        /// </summary>
        /// <returns>Returns true if any error exist.</returns>
        public bool IsExist()
        {
            if (_errors != null &&
                _errors.Count > 0)
            {
                return true;
            }

            return false;
        }

        private BasicError GetNewBasicError(
            string msg,
            ErrorType type,
            string cssClass = "",
            string solution = "",
            string link = "",
            string linkTitle = "")
        {
            var error = new BasicError
            {
                Type                   = type,
                OrderId                = _orderIncrementer++,
                ErrorMessage           = msg,
                Solution               = solution,
                SolutionLink           = link,
                SolutionDisplayMessage = linkTitle,
                CssClass               = cssClass
            };

            return error;
        }

        /// <summary>
        ///     add error message with low priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="cssClass">
        ///     Specific class for this error label.
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </param>
        /// <param name="solution">error solution message</param>
        /// <param name="link">error solution solutionLink</param>
        /// <param name="solutionDisplayMessage">Error solution solutionLink title attribute</param>
        /// <returns>returns the order of the error.</returns>
        public int Add(
            string msg,
            string cssClass = "",
            string solution = "",
            string link = "",
            string solutionDisplayMessage = "") =>
            Add(
                msg,
                ErrorType.Low,
                cssClass,
                solution,
                link,
                solutionDisplayMessage);

        /// <summary>
        ///     add error message with high priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="cssClass">
        ///     Specific class for this error label.
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </param>
        /// <param name="solution">error solution message</param>
        /// <param name="link">error solution solutionLink</param>
        /// <param name="solutionDisplayMessage">Error solution solutionLink title attribute</param>
        /// <returns>returns the order of the error.</returns>
        public int AddHigh(
            string msg,
            string cssClass = "",
            string solution = "",
            string link = "",
            string solutionDisplayMessage = "") =>
            Add(
                msg,
                ErrorType.High,
                cssClass,
                solution,
                link,
                solutionDisplayMessage);

        /// <summary>
        ///     add error message with medium priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="cssClass">
        ///     Specific class for this error label.
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </param>
        /// <param name="solution">error solution message</param>
        /// <param name="link">error solution solutionLink</param>
        /// <param name="solutionDisplayMessage">Error solution solutionLink title attribute</param>
        /// <returns>returns the order of the error.</returns>
        public int AddMedium(
            string msg,
            string cssClass = "",
            string solution = "",
            string link = "",
            string solutionDisplayMessage = "") =>
            Add(
                msg,
                ErrorType.Medium,
                cssClass,
                solution,
                link,
                solutionDisplayMessage);

        /// <summary>
        ///     add error message with given priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="type">Type of your error message.</param>
        /// <param name="cssClass">
        ///     Specific class for this error label.
        ///     By default :
        ///     High : label label-danger high-priority
        ///     Medium : label label-danger medium-priority
        ///     low : label label-warning low-priority
        /// </param>
        /// <param name="solution">error solution message</param>
        /// <param name="solutionLink">error solution solutionLink</param>
        /// <param name="solutionDisplayMessage">Error solution solutionLink title attribute</param>
        /// <returns>returns the order of the error.</returns>
        public int Add(
            string msg,
            ErrorType type,
            string cssClass = "",
            string solution = "",
            string solutionLink = "",
            string solutionDisplayMessage = "")
        {
            var error = GetNewBasicError(
                msg,
                type,
                cssClass,
                solution,
                solutionLink,
                solutionDisplayMessage);

            _errors.Add(error);

            return error.OrderId;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list.</returns>
        public List<string> GetMessages()
        {
            return _errors.Select(n => n.ErrorMessage)
                          .ToList();
        }

        /// <summary>
        ///     Call it when printing it in the view.
        /// </summary>
        /// <returns>Returns all errors ordered by as a string list of html list item ( all classes will be added ).</returns>
        public string GetListItems(string eachItemClasses = "")
        {
            // codes start from here.
            var errorsList = GetErrorsSorted();
            var sb         = new StringBuilder(errorsList.Count * 15);

            foreach (var error in errorsList)
            {
                var cssClass = GetCssClassForError(error);
                sb.Append("<li class='generic-error-item " + eachItemClasses + "'>");
                sb.Append("<span class='" + cssClass + "'>");
                sb.Append(error.ErrorMessage);
                sb.Append("</span>");

                if (!string.IsNullOrEmpty(error.Solution))
                {
                    sb.Append("<span class='" + SolutionStateCssClass + "'>");
                    sb.Append(error.Solution);
                    sb.Append("</span>");
                }

                if (!string.IsNullOrEmpty(error.SolutionLink))
                {
                    sb.Append(
                        "<a class='" +
                        SolutionStateLinkCssClass +
                        "' href='" +
                        error.SolutionLink +
                        "' title='" +
                        error.SolutionDisplayMessage +
                        "'>");

                    sb.Append(error.SolutionDisplayMessage);
                    sb.Append("</a>");
                }

                sb.Append("</li>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as BasicError type.</returns>
        public List<BasicError> GetErrors()
        {
            if (_errors != null &&
                _errors.Count > 0)
            {
                return _errors.ToList();
            }

            return null;
        }

        /// <summary>
        ///     remove msg from the list
        /// </summary>
        public void Remove(int orderId)
        {
            var error = _errors.FirstOrDefault(n => n.OrderId == orderId);

            if (error != null)
            {
                _errors.Remove(error);
            }
        }

        /// <summary>
        ///     remove msg from the list
        /// </summary>
        /// <param name="message"></param>
        public void Remove(string message)
        {
            var error = _errors.FirstOrDefault(n => n.ErrorMessage == message);

            if (error != null)
            {
                _errors.Remove(error);
            }
        }

        /// <summary>
        ///     Clean counter and clean the error list start from 0.
        /// </summary>
        public void Clear()
        {
            _orderIncrementer = 0;
            _errors.Clear();

            //errors.Capacity = defaultCapacity;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns high error message as string list.</returns>
        public List<string> GetMessagesHigh()
        {
            return _errors.Where(n => n.Type == ErrorType.High).Select(n => n.ErrorMessage).ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns low error message as string list.</returns>
        public List<string> GetMessagesLow()
        {
            if (_errors.Count > 0)
            {
                return _errors.Where(n => n.Type == ErrorType.Low).Select(n => n.ErrorMessage).ToList();
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns medium error message as string list.</returns>
        public List<string> GetMessagesMedium()
        {
            if (_errors.Count > 0)
            {
                return _errors.Where(n => n.Type == ErrorType.Medium).Select(n => n.ErrorMessage).ToList();
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list of sorted by order id.</returns>
        public List<string> GetMessagesSorted()
        {
            if (_errors.Count > 0)
            {
                return _errors.OrderBy(n => n.OrderId).Select(n => n.ErrorMessage).ToList();
            }

            return null;
        }

        /// <summary>
        ///     Returns errors in sorted order.
        /// </summary>
        /// <returns>Returns errors in sorted order.</returns>
        public List<BasicError> GetErrorsSorted()
        {
            if (_errors.Count > 0)
            {
                return _errors.OrderBy(n => n.OrderId).ToList();
            }

            return null;
        }
    }
}