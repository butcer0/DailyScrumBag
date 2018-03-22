using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DailyScrumBag.Infrastructure.Constants
{
    public sealed class AppSettingConstant
    {
        #region Connection Settings
        // Database Connection String
        public const string CONNECTION_STRING = "ServiceConfiguration:Setting:DatabaseSetting:ConnectionString";
        #endregion

        #region Admin Email Keys
        public const string ADMIN_EMAIL_NO_QUEUED_EMAILS = "no-queued-emails-exist";
        public const string ADMIN_EMAIL_GENERATED_AUTHOR = "Erik Butcher";
        #endregion

        #region Email Settings
        public const string GMAIL_SMTP_HOST = "smtp.gmail.com";
        public const string GMAIL_SMTP_USERNAME = "dailyscrumbag.noreply@gmail.com";
        public const string GMAIL_SMTP_SENDEREMAIL = "dailyscrumbag.noreply@gmail.com";
        public const string GMAIL_SMTP_PASSWORD = "4d18704da3";
        public const int GMAIL_SMTP_TLS_PORT = 587;
        public const int GMAIL_SMTP_SSL_PORT = 465;
        public const bool GMAIL_SMTP_TLS_SSL_REQUIRED = true;
        public const string GMAIL_SMTP_SENDERNAME = "The Daily ScrumBag";
        public const bool GMAIL_SMTP_ENABLESSL = true;
        public const bool GMAIL_SMTP_ISBODYHTML = true;
        #endregion

        #region Email Templates
        public const string SMTP_DAILY_EMAIL_TEMPLATE = @"\templates\emailtemplates\dailyemailtemplate.html";
        public const string SMTP_ADMIN_EMAIL_TEMPLATE = @"\templates\emailtemplates\dailyemailtemplate.html";
        public const string SMTP_ACTIVATE_DEVICE_EMAIL_TEMPLATE = @"\templates\emailtemplates\dailyemailtemplate.html";
        #endregion
        //
        // Regular expressions
        //
        // use REGEXP_ prefix
        //
        #region Regex Constants
        // Alpha numeric characters validation.
        public const string REGEXP_ALPHANUMERIC_NUMBER = "^[a-zA-Z0-9]*$";

        public const string REGEXP_CUSTOMERADDRESS = @"^[A-Za-z0-9- ,.'#]+$";
        // address can contain alphanumeric chars plus 6 special chars: ' . - , #, / and space
        public const string REGEXP_ADDRESS = "^[A-Za-z0-9- ,.'#\\/]+$";

        // city reg exp does not enforce rules on special chars repetitions. It can identify standard names like Charlotte, multiple words names like White Bear Lake and city names with special chars: O'Fallon, Coeur d'Alene, Winston-Salem, St. Charles.
        //
        // (more complex version which enforces rules on on special chars repetitions but may not be able to cover all cases: ^[a-zA-Z]+(?:[.]{0,1}[ ]{0,1}|[-]{0,1}|[']{0,1}|(?:[ ]{0,1}[a-zA-Z]+[ ]{0,1})*|[ ]{0,1}[a-zA-Z]+[' ]{0,1})[a-zA-Z]+$)
        public const string REGEXP_CITY = "^([a-zA-Z]+[-' .]*)*[a-zA-Z]+$";

        // states in the US postal format (2-char: NC)
        public const string REGEXP_STATE_SHORT = "^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$";

        // short zip code in the US format (28227)
        public const string REGEXP_ZIPCODE_SHORT = @"^\d{5}$";

        public const string REGEXP_ZIPCODE = @"^\d{0,9}$";

        // short or complete zip code in the US format without hyphen (28227 or 282273456)
        public const string REGEXP_ZIPCODE_DIGITS_ONLY = @"^\d{5}(\d{4})?$";

        // complete zip code in the US format with hyphen (28227-3456)
        public const string REGEXP_ZIPCODE_LONG = @"^\d{5}([\-]?\d{4})?$";

        // Standard text field regex validation pattern
        public const string REGEXP_TEXTFIELD_STANDARD = "^[A-Za-z0-9- ,_.'#]+$";

        // Text field regex validation pattern with extra special characters
        //Actual regex in PHP:   /^[A-Za-z0-9- ,:_\."\'#\&%\/\\\(\)]+$/
        public const string REGEXP_TEXTFIELD_EXTENDED = "^[A-Za-z0-9- ,:_\\.\"\\'#\\&%\\/\\\\\\(\\)]+$";

        // Standard filepath field regex validation pattern
        public const string REGEXP_FILEPATH_STANDARD = "^[A-Za-z0-9- _.\\/]+$";

        // Match date time in the ISO 8601 format (e.g., 2014-02-05T12:55:57.000-05:00)
        public const string REGEXP_ISO8601_DATETIME_STANDARD = @"^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[0-1]|0[1-9]|[1-2][0-9])T(2[0-3]|[0-1][0-9]):([0-5][0-9]):([0-5][0-9])(\.[0-9]+)?(Z|[+-](?:2[0-3]|[0-1][0-9]):[0-5][0-9])?$";

        // Match date in the ISO 8601 format (e.g., 2014-02-05)
        public const string REGEXP_ISO8601_DATE_STANDARD = @"^(19|20)\d\d-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$";

        // Match date in the mm/dd/yyyy format (e.g., 05/02/2014)
        public const string REGEXP_DATE_STANDARD = @"^((0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])\/(19|20)?[0-9]{4})$";

        // Standard text field regex validation pattern
        public const string REGEXP_IP_ADDRESS = "^[a-z0-9.:]+$";

        // Standard regex to validate digits
        public const string REGEXP_ONLY_DIGITS = @"^\d+$";

        // Standard regex to validate decimals numbers
        public const string REGEXP_ONLY_DECIMAL_NUMBERS = @"^-*[0-9,\.]+$";

        // Range for remove unprintable characters (allowed: x20 to x7E in ascii table)
        public const string REGEXP_NON_PRINTABLE_ASCII = "[^\x20-\x7E]";

        //Regular Express for serial number validation in Delivery module.
        public const string REGEXP_SERIAL_NUMBER = "^[A-Z0-9]{4}[-]{1}[A-Z0-9]{4}[-]{1}[A-Z0-9]{4}[-]{1}[A-Z0-9]{4}$";

        //
        // Messages
        //
        public const string CSRF_TIMEOUT_MESSAGE = "MerchantSoft Enterprise could not complete this action due to a missing form token. <br /><br />You may have cleared your browser cookies, which could have resulted in the expiration of the current form token. A new form token has been reissued.";

        public const string INACTIVITY_TIMEOUT_MESSAGE = "For security reasons your session has timed out due to inactivity.";

        public const string EMAIL_NO_RESPONSE_MESSAGE = "<em>(Please do not respond to this message... It comes from an unattended mailbox.)</em><br /><br/>";

        //
        // Validation Messages
        //
        // use VALIDATOR_ prefix
        //
        public const string VALIDATOR_INVALID_STANDARD_MESSAGE = "the value specified (%value%) is invalid";

        public const string VALIDATOR_INVALID_ADDRESS_MESSAGE = "the value specified (%value%) contains characters not allowed in a U.S. address. Allowed characters are: letters, numbers, ' . - , #, / and space";

        public const string VALIDATOR_INVALID_CITY_MESSAGE = "the value specified (%value%) contains characters not allowed in a U.S. city name";

        public const string VALIDATOR_INVALID_STATE_SHORT_MESSAGE = "the value specified (%value%) is not a valid U.S. state abbreviation";

        public const string VALIDATOR_INVALID_ZIPCODE_MESSAGE = "the value specified (%value%) is not a valid U.S. zip code";

        public const string VALIDATOR_INVALID_GENDER_MESSAGE = "the value specified (%value%) is invalid. Allowed values are M and F.";

        // must be followed by the max acceptable value
        public const string VALIDATOR_INVALID_DATE_MESSAGE = "the value specified (%value%) is expired or greater than ";

        public const string VALIDATOR_INVALID_ISO8601_TIME_MESSAGE = "the value specified (%value%) is not a time in the ISO 8601 standard";

        public const string VALIDATOR_INVALID_TEXTFIELD_STANDARD_MESSAGE = "Field may only contain alphanumeric characters plus the special characters single quote(\'), period(.), dash(-), space( ), underscore(_)";

        //Actual format in PHP "Field may only contain alphanumeric characters plus the special characters single quote(\'), double quote("), period(.), comma(,), colon(:), dash(-), space( ), underscore(_), hash(#), percent(%), ampersand(&), slash(/), backslash(\), parenthesis(())";
        public const string VALIDATOR_INVALID_TEXTFIELD_EXTENDED_MESSAGE = "Field may only contain alphanumeric characters plus the special characters single quote(\'), double quote(\"), period(.), comma(,), colon(:), dash(-), space( ), underscore(_), hash(#), percent(%), ampersand(&), slash(/), backslash(\\), parenthesis(())";

        public const string VALIDATOR_INVALID_FILEPATH_STANDARD_MESSAGE = "Field may only contain alphanumeric characters plus the special characters period(.), dash(-), space( ), underscore(_), forward slash(/)";

        public const string VALIDATOR_INVALID_NOT_DIGITS_MESSAGE = "Field can only contain numeric digits";

        public const string VALIDATOR_INVALID_FLOAT_AS_CURRENCY = "The value specified (%value%) is an invalid format, it should be like 0,000.00";

        public const string VALIDATOR_INVALID_NOTEMPTY = "Field must not be empty";

        public const string VALIDATOR_INVALID_ERROR_RECORD_FOUND = "The value specified (%value%) already exists";

        public const string VALIDATOR_INVALID_FILENAME = "The value specified (%value%) is not a valid filename. Check for special characters and/or required extension.";

        public const string VALIDATOR_INVALID_DATE_STANDARD = "%value% does not fit the date format \'mm/dd/yyyy\'";

        public const string VALIDATOR_INVALID_DATE_ISO8601 = "%value% does not fit the ISO 8601 date format \'yyyy-mm-dd\'";

        public const string VALIDATOR_INVALID_DATETIME_ISO8601 = "%value% does not fit the ISO 8601 date time format \'yyyy-mm-ddThh:mm:ss\'";

        public const string VALIDATOR_INVALID_IP_ADDRESS_MESSAGE = "%value% is not a valid IPv4 or IPv6 address";

        //
        // Password
        //
        public const string PASSWORD_REQUIREMENTS_DESCRIPTION = "Password must conform to the following requirements:";

        public const string PASSWORD_NUMBERS_REQUIRED_MESSAGE = "<li>At least one number</li>";

        public const string PASSWORD_LETTERS_REQUIRED_MESSAGE = "<li>At least one lowercase character</li>";

        public const string PASSWORD_CAPS_REQUIRED_MESSAGE = "<li>At least one uppercase character</li>";

        public const string PASSWORD_INVALID_FORMAT_MESSAGE = "Invalid password format. Please ensure the password entered conforms to the requirements below.";

        //
        // Error types
        //
        // use ERROR_TYPE prefix
        //
        // identifies error messages to be formated as JSON in the response sent to the client
        public const int ERROR_TYPE_JSON_RESPONSE = 100200;

        public const string ISO8601_DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.fff";
        // Defined static as we can't define date time constant.
        public static readonly DateTime DEFAULT_DATE_TIME = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        #endregion

        //Defines display settings
        #region Display Settings
        public const string SETTING_ON = "ON";
        public const string SETTING_OFF = "OFF";
        #endregion
    }
}
