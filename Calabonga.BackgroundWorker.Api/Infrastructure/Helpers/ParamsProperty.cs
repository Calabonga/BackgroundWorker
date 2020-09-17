using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Calabonga.BackgroundWorker.Api.Infrastructure.Entities;
using Calabonga.EntityFrameworkCore.Entities.Base;
using Calabonga.Microservices.Core.Exceptions;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.Helpers
{
        /// <summary>
    /// Parameters container adds to class additional  to held any parameters 
    /// </summary>
    public abstract class ParamsProperty : Identity
    {
        #region Available parameters

        /// <summary>
        /// Represents any of DateTime as period From
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterDateFrom = "DateFrom";

        /// <summary>
        /// Represents any of DateTime as period To
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterDateTo = "DateTo";

        /// <summary>
        /// Represents any items Total Count
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterTotal = "TotalCount";

        /// <summary>
        /// Represents any of Identifier
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterId = "Identifier";

        /// <summary>
        /// Represents any of UserName
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterUserName = "UserName";

        /// <summary>
        /// Represents any of Host
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterHost = "Host";

        /// <summary>
        /// Represents any of DateTime
        /// Predefined parameter for simplification
        /// </summary>
        public static string ParameterDateTime = "DateTime";

        /// <summary>
        /// Represents any items of collection
        /// Predefined parameter for simplification
        /// </summary>public static string ParameterMessage = "Message";
        public static string ParameterItems = "Items";

        #endregion

        /// <summary>
        /// Serialized to string parameters for work
        /// </summary>
        public string? Parameters { get; private set; }

        /// <summary>
        /// Adds a bunch of parameters for current work
        /// </summary>
        /// <param name="parameters"></param>
        public void AddParameters(IEnumerable<WorkParameter> parameters)
        {
            Parameters = JsonSerializer.Serialize(parameters);
        }

        /// <summary>
        /// Returns parameters for current task work
        /// </summary>
        /// <returns></returns>
        public T GetParamByName<T>(string name)
        {
            if (Parameters == null)
            {
                return default(T)!;
            }

            var parameters = JsonSerializer.Deserialize<IEnumerable<WorkParameter>>(Parameters);
            var parameter = parameters.FirstOrDefault(x => x.Name!.ToLower().Equals(name.ToLower()));
            if (parameter == null)
            {
                return default(T)!;
            }

            var type = Type.GetType(parameter.TypeName);
            if (type!.IsPrimitive)
            {
                if (TypeHelper.CanChangeType(parameter.Value, type))
                {
                    return (T)Convert.ChangeType(parameter.Value, type);
                }
                return (T)parameter.Value;
            }

            switch (type.Name)
            {
                case "Decimal":
                    return (T)Convert.ChangeType(parameter.Value, type);

                case "DateTime":
                    return (T)Convert.ChangeType(parameter.Value, type);

                case "String":
                    var resultString = parameter.Value.ToString();
                    return (T)Convert.ChangeType(resultString!, type);

                case "Guid":
                    var resultGuid = Guid.Parse(parameter.Value.ToString()!);
                    return (T)Convert.ChangeType(resultGuid!, type);

                default:
                    try
                    {
                        return JsonSerializer.Deserialize<T>(parameter.Value.ToString());
                    }
                    catch
                    {
                        throw new MicroserviceArgumentOutOfRangeException();
                    }
            }
        }
    }
}