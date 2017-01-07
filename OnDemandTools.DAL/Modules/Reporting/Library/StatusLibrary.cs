using Microsoft.Extensions.Configuration;
using OnDemandTools.Common.Configuration;
using OnDemandTools.Common.Extensions;
using OnDemandTools.DAL.Modules.Reporting.Model;
using OnDemandTools.DAL.Modules.Reporting.Queries;
using System.Collections.Generic;
using System.Linq;


namespace OnDemandTools.DAL.Modules.Reporting.Library
{
    public static class StatusLibrary
    {
        private static List<DF_StatusEnum> _statusEnums;
        private static AppSettings  _configuration;

        public static List<DF_StatusEnum> StatusEnums
        {
            get
            {
                if (_statusEnums.IsNullOrEmpty())
                {
                    LoadStatusEnums();
                    return _statusEnums;
                }
                else
                    return _statusEnums;
            }

            set
            {
                _statusEnums = value;
            }
        }

        public static void Init(AppSettings configuration)
        {
            _configuration = configuration;           
        }

        static StatusLibrary()
        {
           
        }

        public static void LoadStatusEnums()
        {
            var statusEnumQuery = new StatusEnumsQuery(_configuration);
            _statusEnums = statusEnumQuery.CreateGetStatusEnumsQuery().ToList();
        }

        public static DF_StatusEnum GetStatusEnumByValue(string value)
        {
            var statusEnum = StatusEnums.FirstOrDefault(se => se.Value == value);

            return statusEnum ?? new DF_StatusEnum { Enum = 0, Value = "None" };
        }

        public static DF_StatusEnum GetStatusEnumByEnum(int? sEnum)
        {
            var none = new DF_StatusEnum { Enum = 0, Value = "None" };

            return sEnum != null
                ? StatusEnums.FirstOrDefault(se => se.Enum == sEnum) ?? none
                : none;
        }

        public static List<DF_StatusEnum> GetStatusEnums()
        {
            return StatusEnums;
        }
    }
}
