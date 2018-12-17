using System;
using System.Collections.Generic;

namespace Avanage.SorterFeelLite.UI
{
    public sealed class Constants
    {
        public const string ROLE_ADMIN_OFFICER = "AdminOfficer";
        public const string ROLE_ADMIN_SUPERVISOR = "AdminSupervisor";

        public const string ROLE_VAULT_OFFICER = "VaultOfficer";
        public const string ROLE_VAULT_SUPERVISOR = "VaultSupervisor";

        public const string ROLE_BOXROOM_OFFICER = "BoxRoomOfficer";

        public const string ROLE_PROCESSOR = "Processor";
        public const string ROLE_CASH_PROCESSING_OFFICER = "CashProcessingOfficer";
        public const string ROLE_CASH_PROCESSING_SUPERVISOR = "CashProcessingSupervisor";

        public const string ROLE_TREASURY_OFFICER = "TreasuryOfficer";
        public const string ROLE_TREASURY_SUPERVISOR = "TreasurySupervisor";
    }

    public enum Roles {
        ADMIN_OFFICER = 1,
        ADMIN_SUPERVISOR = 2,
        BOXROOM_OFFICER = 3,
        CASH_PROCESSING_OFFICER = 4,
        CASH_PROCESSING_SUPERVISOR = 5,
        PROCESSOR = 6,
        TREASURY_OFFICER = 7,
        TREASURY_SUPERVISOR = 8,
        VAULT_OFFICER = 9,
        VAULT_SUPERVISOR = 10
    };

    internal static class EnumUtil
    {
        public static List<KeyValuePair<string, string>> ToKeyValuePairs(Type enumObj)
        {
            var keyPair = new List<KeyValuePair<string, string>>();
            foreach (var e in Enum.GetValues(enumObj))
            {
                var key = Convert.ToInt16(e).ToString();
                var val = e.ToString();
                keyPair.Add(new KeyValuePair<string, string>(key, val));
            }
            return keyPair;
        }
    }
}

