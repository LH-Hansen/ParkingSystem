using System;
using System.Dynamic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Linq;

namespace ParkeringsSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ParkeringSystem();                                                                                   //Instanciere hele programmet.
        }

        static void ParkeringSystem()
        {
            string[] nummerplade = new string[100];
            string klokken = DateTime.Now.ToString();

            DefualtArray(nummerplade);
            SystemMenu(nummerplade, klokken);
        }

        static void DefualtArray(string[] modtagetNummerplade)
        {
            for (int i = 0; i < modtagetNummerplade.Length; i++)                                                 //Bruger en for loop til at sætte alle strings i array til "0".
                modtagetNummerplade[i] = "0";
        }

        static void SystemMenu(string[] modtagetNummerplade, string modtagetKlokken)
        {
            int hovedMenu;

            while (true)
            {
                ResetNummerplade(modtagetNummerplade, modtagetKlokken);                                         //Ser på tiden om array skal resetes.

                Console.Clear();
                Console.Write("Parkeringssystem menu\n\n1. Registrer nummerplade\n\n2. Afmelding af nummerplade\n\n3. P-vagt menu\n\nMenu Valg: ");
                hovedMenu = Convert.ToInt32(Console.ReadLine());

                switch (hovedMenu)                                                                              //Første switch case der giver adgang til brugerfalde for både alm. brugere men også pvagter.
                {
                    case 1:
                        RegistrerNummerplade(modtagetNummerplade);                                              //Kalder metode der opretter og gemmer nummerplader i et array.
                        break;
                    case 2:
                        AfmeldNummerplade(modtagetNummerplade);                                                 //Kalder metode der sletter en given nummerplade fra arrayet hvis den findes i systemet.
                        break;                                                                                  //Findes den ikke i systemet vil det blive meldt.
                    case 3:
                        PvagtMenuLogin(modtagetNummerplade, modtagetKlokken);
                        break;
                }
            }
        }

        static void ResetNummerplade(string[] modtagetNummerplade, string modtagetKlokken)
        {
            DateTime tidNu = DateTime.Now;                                                                       //Metode opretter ny DateTime.Now hver gang den kaldes for at se den mest opdaterede tid.

            if (tidNu.Hour == 12 && tidNu.Minute == 28)                                                          //Kontrollerer om klokken er 02:00.
            {
                DefualtArray(modtagetNummerplade);                                                               //Hvis tiden er det, så klades defaultarray metoden og sætter alle strings til defualt værdien.
                UdeAfDriftDisplay();                                                                             //Kalder ude af drift metode, så hvis nogen skulle bruge den mens den resetter så ved de når den er klar igen.
            }
        }

        static void UdeAfDriftDisplay()
        {
            for (int i = 0; i < 45; i++)
            {
                string modtagetKlokken = DateTime.Now.ToString("HH:mm");                                                    //Laver klokken om til læsbar string.
                Console.WriteLine("Parkeringsystem nulstilles. Ude af drift mellem 02:00 og 02:01\n\nKlokken er {0}", modtagetKlokken);
                Thread.Sleep(1000);
            }
        }

        static void RegistrerNummerplade(string[] modtagetNummerplade)
        {
            for (int i = 0; i < modtagetNummerplade.Length; i++)                                                 //Laver en for loop der gemmer strings i array.
            {
                Console.Clear();
                Console.Write("Velkommen til EUC's parkeringssystem\n\nIndtast din nummerplade: ");

                if (modtagetNummerplade[i] != "0") { }                                                           //If statement kigger om pladsen i arrayet er brugt. Hvis det er springes den over.
                else
                {
                    modtagetNummerplade[i] = Console.ReadLine();                                                 //Hvis den ikke er, så ændres den til hvad der blev indtastet.
                    Console.Write("Nummerplade '{0}' er nu registreret!", modtagetNummerplade[i]);
                    Afslut();

                    break;
                }
            }
        }

        static void AfmeldNummerplade(string[] modtagetNummerplade)                                              //En metode til at afmelde sin nummerplade.
        {
            string nummerpladeReset;
            bool bilRegistretet = false;

            Console.Clear();
            Console.Write("Afmelding af nummerplade\n\nIndtast din nummerplade: ");
            nummerpladeReset = Console.ReadLine();                                                               //Opretter nummerplade der skal slettes.

            for (int i = 0; i < modtagetNummerplade.Length; i++)                                                 //Kører array igennem for at finde en nummerplade der matcher.
            {
                if (nummerpladeReset == modtagetNummerplade[i])                                                  //Hvis den søgte nummerplade finder et match i array'et-
                {
                    modtagetNummerplade[i] = "0";                                                                //-sættes den til "0" (default værdi).
                    bilRegistretet = true;                                                                       //laver bool til true.
                    break;
                }
            }

            if (bilRegistretet == true)                                                                          //Hvis bilen er fundet og slettet melder den det til brugeren.
            {
                Console.WriteLine("Nummerplade '{0}' er nu afmeldt. Ha' en god dag.", nummerpladeReset);
                Afslut();
            }
            else                                                                                                 //Hvis den ikke blev fundet meldes dette også.
            {
                Console.WriteLine("Nummerplade '{0}' er ikke registreret.", nummerpladeReset);
                Afslut();
            }
        }

        static bool CheckLogin(string modtagetIndtastetBrugernavn, string modtagetIndtastetPassword)             //Metode til at se om login informationerne er korrekt. 
        {
            bool modtagetLoginKorrekt = false;                                                                   //Som defualt sættes bool til false.
            string brugernavn = "admin", password = "admin1";                                                    //Her oprettes brugernavn og password.

            if (modtagetIndtastetBrugernavn == brugernavn && password == modtagetIndtastetPassword)              //Kontrolerer om indtastede informationer matcher brugernavn og password.
            {
                modtagetLoginKorrekt = true;                                                                     //Hvis det gør, søtte bool til true.
                return modtagetLoginKorrekt;                                                                     //Bool true returneres her.
            }
            else
                return modtagetLoginKorrekt;                                                                     //Bool false reurnres hvis det ikke matcher.
        }

        static void PvagtMenuLogin(string[] modtagetNummerplade, string modtagetKlokken)
        {
            string indtastetBrugernavn, indtastetPassword;
            int antalLoginForsøg = 3;
            bool loginKorrekt;

            for (int i = 0; i < antalLoginForsøg; i++)                                                           //For loop der tæller antal forsøg til login.
            {
                Console.Clear();
                Console.Write("P-vagt login\n\nIndtast brugernavn: \t");
                indtastetBrugernavn = Console.ReadLine();

                Console.Write("Indtast password: \t");
                indtastetPassword = Console.ReadLine();

                loginKorrekt = CheckLogin(indtastetBrugernavn, indtastetPassword);                               //Kalder metode der returner en bool alt efter om p-vagt login er korrekt.

                if (loginKorrekt == false)                                                                       //If statement der bruger den returnerede bool for at se om login er rigtig eller forkert.
                {
                    Console.WriteLine("\nForkert brugernavn/password, {0} forsøg tilbage", 2 - i);               //Priner antal login forsøg tilbage efter fejl.
                    Thread.Sleep(2000);
                    Console.Clear();
                }
                else
                {
                    PVagtMenu(modtagetNummerplade, modtagetKlokken);
                    break;
                }
            }
        }

        static void PVagtMenu(string[] modtagetNummerplade, string modtagetKlokken)                              //Pvagt menu til anden switch case
        {
            int pvagtMenu;

            do
            {
                ResetNummerplade(modtagetNummerplade, modtagetKlokken);                                         //Kontrolrer klokken om array skal resetes.

                Console.Clear();
                Console.Write("P-Vagt menu\n\n1. Liste over registrerede nummerplader\n\n2. Søg efter nummerplade\n\n3. Forlad P-Vagt menu\n\nMenu valg: ");
                pvagtMenu = Convert.ToInt32(Console.ReadLine());

                switch (pvagtMenu)                                                                             //En switch case over p-vagt funktioner
                {
                    case 1:
                        ListeOverNummerplader(modtagetNummerplade);                                            //Kalder metode der viser alle registrerede nummerplader.                                                  
                        break;
                    case 2:
                        NummerpladeSøgningOgTimer(modtagetNummerplade);                                       //Kalder metode der kan søge efter ikke-/registrerede nummerplader i systemet.
                        break;                                                                                //Metoden har også en inbygget timer hvis nummerplden ikke er registreret.
                }
            } while (pvagtMenu < 3);
        }

        static void ListeOverNummerplader(string[] modtagetNummerplade)                                      //Metode der udskriver registrerede biller i array.
        {
            Console.Clear();
            Console.WriteLine("Liste over registrerede nummerplader \n\nRegistrerede nummerplader:\n");

            for (int i = 0; i < modtagetNummerplade.Length; i++)                                            //Kører array igennem
            {
                if (modtagetNummerplade[i] != "0")
                    Console.WriteLine(modtagetNummerplade[i]);
            }
        }

        static void NummerpladeSøgningOgTimer(string[] modtagetNummerplade)                                      //Metode til at søge efter en given nummerplade for at se om den er registreret.
        {
            string nummerpladeSøgning;

            Console.Clear();
            Console.Write("Søg efter nummerplade/start timer\n\nSøg nummerplade: ");
            nummerpladeSøgning = Console.ReadLine();

            if (modtagetNummerplade.Contains(nummerpladeSøgning))
            {
                Console.WriteLine("Nummerplade '{0}' er registreret!", nummerpladeSøgning);
                Afslut();
            }
            else
                IkkeRegistreret(nummerpladeSøgning);
        }

        static void IkkeRegistreret(string modtagetNummerpladesøgning)
        {
            string bødeudskrivningString;
            DateTime bødeudskrivning = DateTime.Now;

            bødeudskrivning = bødeudskrivning + TimeSpan.Parse("00:20:00");
            bødeudskrivningString = bødeudskrivning.ToString("HH:mm");

            Console.Clear();
            Console.Write("Søg efter nummerplade\n\nNummerplade '{0}' er ikke registreret. Bøde kan udskrives klokken: {1}", modtagetNummerpladesøgning, bødeudskrivningString);
            Afslut();
        }

        static void Afslut()
        {
            Console.Write("\n\nTryk enter for at afslutte: ");
            Console.ReadLine();
        }
    }
}
