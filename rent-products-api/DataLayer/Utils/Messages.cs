using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rent_products_api.DataLayer.Utils
{
    public class Message
    {
        public string Text { get; set; }
        public string MessageType { get; set; }
    }

    public static class Messages
    {
        public static Message Message_LoggedInError = new Message { Text = "Utilizatorul nu a putut fi logat. Verificați informațiile introduse.", MessageType = MessageType.Error };
        public static Message Message_LoggedInSuccessfully = new Message { Text = "Autentificarea s-a făcut cu succes.", MessageType = MessageType.Success };
        public static Message Message_RefreshedTokenSuccess = new Message { Text = "Token-ul a fost actualizat.", MessageType = MessageType.Success };
        public static Message Message_RefreshedTokenError = new Message { Text = "Token-ul nu a putut fii actualizat.", MessageType = MessageType.Error };
        public static Message Message_RevokeTokenSuccess = new Message { Text = "Token-ul a fost revocat.", MessageType = MessageType.Success };
        public static Message Message_RevokeTokenError = new Message { Text = "Token-ul nu a putut fii revocat.", MessageType = MessageType.Error };
        public static Message Message_ConfigurePasswordError = new Message { Text = "Nu s-a putut actualiza parola.", MessageType = MessageType.Info };

        public static Message Message_ValidateResetTokenSuccess = new Message { Text = "S-a validat token-ul cu succes.", MessageType = MessageType.Success };
        public static Message Message_ValidateResetTokenError = new Message { Text = "Token-ul nu a putut fi validat.", MessageType = MessageType.Error };

        public static Message Message_UserRegisteredSuccess = new Message { Text = "Utilizatorul a fost înregistrat cu succes.", MessageType = MessageType.Success };
        public static Message Message_UserRegisterError = new Message { Text = "Utilizatorul nu a putut fi înregistrat.", MessageType = MessageType.Error };
        public static Message Message_UserUpdateSuccess = new Message { Text = "Au fost schimbate datele utilizatorului.", MessageType = MessageType.Success };
        public static Message Message_UserUpdateError = new Message { Text = "Datele utilizatorului nu au putut fi schimbate.", MessageType = MessageType.Error };
        public static Message Message_UserDeleteSuccess = new Message { Text = "Utilizatorul a fost șters.", MessageType = MessageType.Success };
        public static Message Message_UserDeleteError = new Message { Text = "Utilizatorul nu a putut fi șters.", MessageType = MessageType.Error };       

        public static Message Message_EmailAlreadyUsed = new Message { Text = "Acest email este deja folosit.", MessageType = MessageType.Info };
        

        public static Message Message_EmailVerified = new Message { Text = "Email verificat cu succes.", MessageType = MessageType.Success };
        public static Message Message_EmailVerifyError = new Message { Text = "Email-ul nu a putut fi verificat.", MessageType = MessageType.Error };
        public static Message Message_SendVerificationEmailError = new Message { Text = "Nu s-a putut trimite un email de verificare. Va rugăm verificați email-ul introdus.", MessageType = MessageType.Error };
        public static Message Message_SendVerificationEmailSuccess = new Message { Text = "Un email de verificare a contului a fost trimis la adresa dumneavoastră.", MessageType = MessageType.Success };

        public static Message Message_ForgottenPasswordEmailSent = new Message { Text = "A fost trimis un email pentru modificarea parolei dumneavoastră.", MessageType = MessageType.Success };
        public static Message Message_ForgottenPasswordEmailNotSent = new Message { Text = "Email-ul pentru modificarea parolei nu a putut fi trimis.", MessageType = MessageType.Error };
        public static Message Message_ResetPasswordSuccess = new Message { Text = "Parola a fost schimbată cu succes.", MessageType = MessageType.Success };
        public static Message Message_ResetPasswordError = new Message { Text = "Nu s-a putut schimba parola.", MessageType = MessageType.Error };

        public static Message Message_UserDataLoadError = new Message { Text = "Datele nu au putut fi încărcate.", MessageType = MessageType.Error };
        public static Message Message_UsersLoadError = new Message { Text = "Utilizatorii nu au putut fi încărcati.", MessageType = MessageType.Error };
        public static Message Message_UserNotFound = new Message { Text = "Utilizatorul nu a fost găsit.", MessageType = MessageType.Info };

        public static Message Message_MesajGenericTest = new Message { Text = "Acesta este un mesaj generic.", MessageType = MessageType.Info };
        public static Message Message_MesajGenericTestError = new Message { Text = "Acesta este un mesaj generic de eroare.", MessageType = MessageType.Error };

        #region product messages

        public static Message Message_AddProductError = new Message { Text = "Nu a putut fi adaugat produsul.", MessageType = MessageType.Error };
        public static Message Message_AddProductSuccess = new Message { Text = "Produs adaugat.", MessageType = MessageType.Success };


        public static Message Message_DeleteProductError = new Message { Text = "Produsul nu a putut fi sters.", MessageType = MessageType.Error };
        public static Message Message_DeleteProductSuccess = new Message { Text = "Produs sters.", MessageType = MessageType.Success };

        public static Message Message_GetProductsError = new Message { Text = "Produsele nu au putut fi afisate.", MessageType = MessageType.Error };
        public static Message Message_RentProductError = new Message { Text = "Nu s-a putut inregistra inchirierea.", MessageType = MessageType.Error };
        public static Message Message_RentProductSuccess = new Message { Text = "Rezervare facuta cu succes.", MessageType = MessageType.Success };

        public static Message Message_CancelRentError = new Message { Text = "Rezervarea nu a putut fi anulată.", MessageType = MessageType.Error };
        public static Message Message_CancelRentSuccess = new Message { Text = "Rezervare anulată cu succes.", MessageType = MessageType.Success };

        public static Message Message_ConfirmRentError = new Message { Text = "Rezervarea nu a putut fi confirmată.", MessageType = MessageType.Error };
        public static Message Message_ConfirmRentSuccess = new Message { Text = "Rezervare confirmată cu succes.", MessageType = MessageType.Success };
        #endregion


    }
}
