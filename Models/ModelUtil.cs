namespace LadowebservisMVC.Controllers
{
    public class ModelUtil
    {
        public const string requiredErrMessage_Sk = "Pole '{0}' musí byť zadané.";
        public const string invalidEmailErrMessage_Sk = "Neplatná e-mailová adresa.";
        public const string invalidPhoneErrMessage_Sk = "Neplatný formát telefónneho čísla.";


        public const string requiredErrMessage_Cs = "Pole '{0}' musí být zadané.";
        public const string invalidEmailErrMessage_Cs = "Neplatná e-mailová adresa.";

        public const string requiredErrMessage_En = "Item '{0}' is required.";
        public const string invalidEmailErrMessage_En = "Invalid e-mail address.";

        public const string requiredErrMessage_De = "'{0}' ist erforderlich.";
        public const string invalidEmailErrMessage_De = "Ungültige E-Mail-Adresse.";

        public const string requiredErrMessage_Fr = "Le champ '{0}' est requis.";
        public const string invalidEmailErrMessage_Fr = "Adresse e-mail invalide.";

        public const string requiredErrMessage_Ru = "Поле '{0}' обязательно.";
        public const string invalidEmailErrMessage_Ru = "Неверный адрес электронной почты.";

        public const string phoneRegex = @"^\+4219\d{8}$";
    }
}