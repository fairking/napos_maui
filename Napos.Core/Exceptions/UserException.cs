using Napos.Core.Helpers;
using System;
using System.Linq.Expressions;

namespace Napos.Core.Exceptions
{
    /// <summary>
    /// The purpose of this exception is to notify users about incorrect data provided by them.
    /// It usually goes along with http code 400 (see UserErrorResult) or it could occure when the service executed outside of the request (background job). 
    /// In this case we might consider to log it as an warning or as an error, depended on the situation.
    /// </summary>
    public class UserException : Exception
    {
        /// <summary>
        /// Form fields or any other view model property names (optional)
        /// </summary>
        public string[] Properties { get; protected set; }

        /// <summary>
        /// Validation user messages
        /// </summary>
        public string[] Messages { get; protected set; }

        public UserException(string message) : base(message)
        {
            Properties = new[] { (string)null };
            Messages = new[] { message };
        }

        public UserException(string propertyPath, string message) : base(message + (!string.IsNullOrEmpty(propertyPath) ? " (" + propertyPath + ")" : ""))
        {
            Properties = new[] { propertyPath };
            Messages = new[] { message };
        }

        public UserException(string[] propertyPaths, string[] messages) : base(string.Join("; ", messages) + (propertyPaths != null && propertyPaths.Length > 0 ? " (" + string.Join(", ", propertyPaths) + ")" : ""))
        {
            Properties = propertyPaths;
            Messages = messages;
        }
    }

    public class UserException<TModel, TProperty> : UserException
    {
        public UserException(Expression<Func<TModel, TProperty>> propertyPath, string message) : base(message + (!string.IsNullOrEmpty(propertyPath.GetPropertyPath()) ? " (" + propertyPath.GetPropertyPath() + ")" : ""))
        {
            Properties = new[] { propertyPath.GetPropertyPath() };
            Messages = new[] { message };
        }
    }
}
