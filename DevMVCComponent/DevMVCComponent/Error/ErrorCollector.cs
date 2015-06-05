#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using DevMVCComponent.DataTypeFormat;
using DevMVCComponent.Enums;

#endregion

namespace DevMVCComponent.Error {
    /// <summary>
    /// </summary>
    public class ErrorCollector : IDisposable {
        /// <summary>
        /// </summary>
        public const string SolutionStateLinkClass = "rounded-3 label label-info error-solution-link-color";

        /// <summary>
        /// </summary>
        public const string SolutionStateClass = "rounded-3 label label-success";

        private const string HighRisk = "rounded-3 label label-danger";
        private const string LowRisk = "rounded-3 label label-warning low-error-color";
        private const string MidRisk = "rounded-3 label label-danger mid-error-color";
        private int _defaultCapacity = 60;
        private List<BasicError> _errors;
        private short _orderIncrementer;

        /// <summary>
        /// </summary>
        /// <param name="def"></param>
        public ErrorCollector(int def = 60) {
            _errors = new List<BasicError>(def);
            _defaultCapacity = def;
        }

        public void Dispose() {
            _errors = null;
            GC.Collect();
        }

        public string GetCssForError(BasicError e) {
            if (e.Type == ErrorType.High) {
                return HighRisk;
            }
            if (e.Type == ErrorType.Medium) {
                return MidRisk;
            }
            if (e.Type == ErrorType.Low) {
                return LowRisk;
            }
            return LowRisk;
        }

        /// <summary>
        ///     Is any error exist in the list?
        /// </summary>
        /// <returns>Returns true if any error exist.</returns>
        public bool IsExist() {
            if (_errors != null && _errors.Count > 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     add error message with low priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="quantityTypeIsNotValidPleaseSelectAValidQuantityType"></param>
        /// <param name="solution"></param>
        public int Add(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                OrderId = _orderIncrementer++,
                ErrorMessage = msg,
                Type = ErrorType.Low
            };
            _errors.Add(error);
            return error.OrderId;
        }

        /// <summary>
        ///     add error message with high priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="solution"></param>
        public int AddHigh(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.High,
                OrderId = _orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            _errors.Add(error);
            return error.OrderId;
        }

        /// <summary>
        ///     add error message with medium priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="solution"></param>
        public int AddMedium(string msg, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.Medium,
                OrderId = _orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            _errors.Add(error);
            return error.OrderId;
        }

        /// <summary>
        ///     add error message with given priority
        /// </summary>
        /// <param name="msg">set your message.</param>
        /// <param name="type">Type of your error message.</param>
        public int Add(string msg, ErrorType type, string solution = "", string link = "", string linkTitle = "") {
            var error = new BasicError {
                Type = ErrorType.Low,
                OrderId = _orderIncrementer++,
                ErrorMessage = msg,
                Solution = solution,
                Link = link,
                LinkTitle = linkTitle
            };
            _errors.Add(error);
            return error.OrderId;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list.</returns>
        public List<string> GetMessages() {
            return _errors.Select(n => n.ErrorMessage)
                .ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as Error Object.</returns>
        public List<BasicError> GetErrors() {
            if (_errors != null && _errors.Count > 0) {
                return _errors.ToList();
            }
            return null;
        }

        public void Remove(int orderId) {
            var error = _errors.FirstOrDefault(n => n.OrderId == orderId);
            if (error != null) {
                _errors.Remove(error);
            }
        }

        public void Remove(string message) {
            var error = _errors.FirstOrDefault(n => n.ErrorMessage == message);
            if (error != null) {
                _errors.Remove(error);
            }
        }

        /// <summary>
        ///     Clean counter and clean the error list start from 0.
        /// </summary>
        public void Clear() {
            _orderIncrementer = 0;
            _errors.Clear();
            //errors.Capacity = defaultCapacity;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns high error message as string list.</returns>
        public List<string> GetMessagesHigh() {
            return _errors.Where(n => n.Type == ErrorType.High).Select(n => n.ErrorMessage).ToList();
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns low error message as string list.</returns>
        public List<string> GetMessagesLow() {
            if (_errors.Count > 0) {
                return _errors.Where(n => n.Type == ErrorType.Low).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns medium error message as string list.</returns>
        public List<string> GetMessagesMedium() {
            if (_errors.Count > 0) {
                return _errors.Where(n => n.Type == ErrorType.Medium).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>Returns all error message as string list of sorted by order id.</returns>
        public List<string> GetMessagesSorted() {
            if (_errors.Count > 0) {
                return _errors.OrderBy(n => n.OrderId).Select(n => n.ErrorMessage).ToList();
            }
            return null;
        }
    }
}