using System;
using System.Collections.Generic;

namespace CaisseEnregistreuse
{
    public class Espece
    {
        public int IdEspece { get; set; }
        public decimal ValeurFaciale { get; set; }

        public Espece(int idEspece, decimal valeurFaciale)
        {
            IdEspece = idEspece;
            ValeurFaciale = valeurFaciale;
        }
    }

    public class FondDeCaisse
    {
        public int IdFondDeCaisse { get; set; }
        public decimal Montant { get; set; }
        public DateTime Journee { get; set; }

        public FondDeCaisse(int idFondDeCaisse, decimal montant, DateTime journee)
        {
            IdFondDeCaisse = idFondDeCaisse;
            Montant = montant;
            Journee = journee;
        }
    }

    public class MiseEnCoffre
    {
        public int IdMiseEnCoffre { get; set; }
        public DateTime DateHeure { get; set; }

        public MiseEnCoffre(int idMiseEnCoffre, DateTime dateHeure)
        {
            IdMiseEnCoffre = idMiseEnCoffre;
            DateHeure = dateHeure;
        }
    }

    public class Caisse
    {
        public List<Produit> ListeProduits { get; private set; }
        public decimal MontantTotal { get; private set; }
        public decimal MontantEnCaisse { get; private set; }
        public decimal MontantEnCoffre { get; private set; }
        public List<Espece> ListeEspeces { get; private set; }
        public List<FondDeCaisse> ListeFondsDeCaisse { get; private set; }
        public List<MiseEnCoffre> ListeMisesEnCoffre { get; private set; }

        public Caisse()
        {
            ListeProduits = new List<Produit>();
            MontantTotal = 0;
            MontantEnCaisse = 0;
            MontantEnCoffre = 0;
            ListeEspeces = new List<Espece>();
            ListeFondsDeCaisse = new List<FondDeCaisse>();
            ListeMisesEnCoffre = new List<MiseEnCoffre>();
        }

        public void AjouterProduit(Produit produit)
        {
            ListeProduits.Add(produit);
            Console.WriteLine($"Produit ajouté : {produit.Libelle}");
        }

        public decimal CalculerMontantTotal()
        {
            decimal montantTotal = 0;

            foreach (Produit produit in ListeProduits)
            {
                montantTotal += produit.CalculerPrixTotal(1); // On suppose ici une quantité de 1 pour chaque produit
            }

            MontantTotal = montantTotal; // Mettre à jour le MontantTotal de la caisse
            Console.WriteLine($"Montant total calculé : {montantTotal} euros");
            return montantTotal;
        }

        public void RecevoirPaiement(decimal montant)
        {
            if (montant < MontantTotal)
            {
                Console.WriteLine("Le montant reçu est insuffisant pour payer la totalité de la caisse.");
                MontantTotal -= montant; // Mettre à jour le MontantTotal
            }
            else if (montant >= MontantTotal)
            {
                Console.WriteLine("La caisse a été payée en totalité.");
                MontantEnCaisse += MontantTotal;
                MontantTotal = 0;
            }
        }

        public decimal RendreMonnaie(decimal montantPaye)
        {
            decimal montantAvecTaxes = CalculerMontantTotal(); // Calculer le montant total des produits
            if (montantPaye < montantAvecTaxes)
            {
                Console.WriteLine("Le montant payé est insuffisant pour effectuer le paiement.");
                return 0; // Renvoyer 0 car aucune monnaie n'est rendue
            }
            else
            {
                decimal monnaieRendue = montantPaye - montantAvecTaxes; // Calculer la monnaie rendue correctement
                MontantEnCaisse += montantAvecTaxes;
                MontantTotal = 0; // La caisse est payée en totalité
                Console.WriteLine($"Monnaie rendue : {monnaieRendue} euros");
                return monnaieRendue;
            }
        }

        public void AjouterEspece(Espece espece)
        {
            ListeEspeces.Add(espece);
            MontantEnCaisse += espece.ValeurFaciale;
            Console.WriteLine($"Ajout de {espece.ValeurFaciale} euros en espèces.");
        }

        public void RetirerEspece(decimal montant)
        {
            if (montant <= MontantEnCaisse)
            {
                MontantEnCaisse -= montant;
                Console.WriteLine($"Retrait de {montant} euros en espèces.");
            }
            else
            {
                Console.WriteLine("Fonds insuffisants en caisse pour effectuer le retrait.");
            }
        }

        public void EffectuerMiseEnCoffre(decimal montant)
        {
            if (montant <= MontantEnCaisse)
            {
                MontantEnCaisse -= montant;
                MontantEnCoffre += montant;
                DateTime maintenant = DateTime.Now;
                MiseEnCoffre miseEnCoffre = new MiseEnCoffre(ListeMisesEnCoffre.Count + 1, maintenant);
                ListeMisesEnCoffre.Add(miseEnCoffre);
                Console.WriteLine($"Mise en coffre de {montant} euros à {maintenant}.");
            }
            else
            {
                Console.WriteLine("Fonds insuffisants en caisse pour effectuer la mise en coffre.");
            }
        }

        public void GenererTicket(decimal montantPaye, List<Produit> produitsAchetes, TypePaiement typePaiement, Ticket ticket)
        {
            Console.WriteLine("\nTicket de caisse :");
            Console.WriteLine($"Date et heure : {ticket.DateHeure}");
            Console.WriteLine("Produits achetés :");
            foreach (var produit in produitsAchetes)
            {
                Console.WriteLine($"{produit.Libelle} - {produit.PrixUnitaire} euros");
            }
            Console.WriteLine($"Montant total : {montantPaye} euros");
            Console.WriteLine($"Mode de paiement : {typePaiement.Libelle}");
            Console.WriteLine($"Merci de votre achat !\n");
        }
    }

    public class Famille
    {
        public int IdFamille { get; set; }
        public string NomFamille { get; set; }
        public string DescriptionFamille { get; set; }

        public Famille(int idFamille, string nomFamille, string descriptionFamille)
        {
            IdFamille = idFamille;
            NomFamille = nomFamille;
            DescriptionFamille = descriptionFamille;
        }
    }

    public class Produit
    {
        public int IdProduit { get; set; }
        public string Libelle { get; set; }
        public string DescriptionProduit { get; set; }
        public decimal PrixUnitaire { get; set; }
        public string CodeBarre { get; set; }

        public Produit(int idProduit, string libelle, string descriptionProduit, decimal prixUnitaire, string codeBarre)
        {
            IdProduit = idProduit;
            Libelle = libelle;
            DescriptionProduit = descriptionProduit;
            PrixUnitaire = prixUnitaire;
            CodeBarre = codeBarre;
        }

        public decimal CalculerPrixTotal(int quantite)
        {
            if (quantite < 0)
            {
                throw new ArgumentException("La quantité ne peut pas être négative.", nameof(quantite));
            }

            return PrixUnitaire * quantite;
        }
    }

    public class Stock
    {
        public int IdStock { get; set; }
        public string NomStock { get; set; }
        public string DescriptionStock { get; set; }

        public Stock(int idStock, string nomStock, string descriptionStock)
        {
            IdStock = idStock;
            NomStock = nomStock;
            DescriptionStock = descriptionStock;
        }
    }

    public class Stocker
    {
        public int IdProduit { get; set; }
        public int IdStock { get; set; }
        public int Quantite { get; set; }

        public Stocker(int idProduit, int idStock, int quantite)
        {
            IdProduit = idProduit;
            IdStock = idStock;
            Quantite = quantite;
        }
    }

    public class Ticket
    {
        public int IdTicket { get; set; }
        public DateTime DateHeure { get; set; }

        public Ticket(int idTicket, DateTime dateHeure)
        {
            IdTicket = idTicket;
            DateHeure = dateHeure;
        }
    }

    public class TypePaiement
    {
        public int IdTypePaiement { get; set; }
        public string Libelle { get; set; }

        public TypePaiement(int idTypePaiement, string libelle)
        {
            IdTypePaiement = idTypePaiement;
            Libelle = libelle;
        }
    }

    public class Paiement
    {
        public decimal Montant { get; private set; }
        public TypePaiement TypePaiement { get; private set; }
        public Ticket Ticket { get; private set; }

        public Paiement(decimal montant, TypePaiement typePaiement, Ticket ticket)
        {
            Montant = montant;
            TypePaiement = typePaiement;
            Ticket = ticket;
            Console.WriteLine($"Paiement de {montant} euros effectué par {typePaiement.Libelle}. Ticket n°{ticket.IdTicket} généré.");
        }
    }

    class Program
    {
        static void Main()
        {
            // Création d'une instance de la caisse
            Caisse caisse = new Caisse();

            // Création de produits, d'une famille, d'un stock et d'un stocker (pour ajouter des produits au stock)
            Famille famille1 = new Famille(1, "Famille A", "Description de la famille A");
            Produit produit1 = new Produit(1, "Produit 1", "Description du produit 1", 9.99m, "123456789");
            Produit produit2 = new Produit(2, "Produit 2", "Description du produit 2", 5.99m, "987654321");
            Stock stock1 = new Stock(1, "Stock principal", "Description du stock principal");
            Stocker stocker1 = new Stocker(1, 1, 50);

            // Création d'un ticket
            Ticket ticket1 = new Ticket(1, DateTime.Now);

            // Demande à l'utilisateur de choisir le mode de paiement
            Console.WriteLine("Choisissez le mode de paiement (1 pour espèces, 2 pour chèques, 3 pour carte bancaire): ");
            int choixPaiement = int.Parse(Console.ReadLine());

            TypePaiement typePaiement1;

            switch (choixPaiement)
            {
                case 1:
                    typePaiement1 = new TypePaiement(1, "espèces");
                    break;
                case 2:
                    typePaiement1 = new TypePaiement(2, "chèques");
                    break;
                case 3:
                    typePaiement1 = new TypePaiement(3, "carte bancaire");
                    break;
                default:
                    Console.WriteLine("Mode de paiement invalide, utilisez espèces par défaut.");
                    typePaiement1 = new TypePaiement(1, "espèces");
                    break;
            }

            // Demande à l'utilisateur de choisir les produits à acheter
            List<Produit> produitsAchetes = new List<Produit>();

            while (true)
            {
                Console.WriteLine("Choisissez un produit à acheter (1 pour Produit 1, 2 pour Produit 2, 0 pour terminer) : ");
                int choixProduit = int.Parse(Console.ReadLine());

                if (choixProduit == 0)
                {
                    break;
                }

                Produit produitAchete = null;

                // Sélectionner le produit en fonction du choix de l'utilisateur
                switch (choixProduit)
                {
                    case 1:
                        produitAchete = produit1;
                        break;
                    case 2:
                        produitAchete = produit2;
                        break;
                    default:
                        Console.WriteLine("Produit invalide.");
                        break;
                }

                if (produitAchete != null)
                {
                    produitsAchetes.Add(produitAchete);
                    Console.WriteLine($"Produit ajouté au panier : {produitAchete.Libelle}");
                }
            }

            // Ajouter les produits au panier
            foreach (var produit in produitsAchetes)
            {
                caisse.AjouterProduit(produit);
            }

            // Calculer le montant total des produits
            decimal montantAPayer = caisse.CalculerMontantTotal();

            // Effectuer un paiement
            Console.WriteLine("Entrez le montant du paiement : ");
            decimal montantPaiement1 = decimal.Parse(Console.ReadLine());
            Paiement paiement1 = new Paiement(montantPaiement1, typePaiement1, ticket1);

            // Gérer le paiement et la monnaie rendue
            if (montantPaiement1 < montantAPayer)
            {
                Console.WriteLine("Le montant payé est insuffisant pour effectuer le paiement.");
            }
            else
            {
                caisse.RecevoirPaiement(montantPaiement1);
                montantAPayer -= montantPaiement1;

                // Calculer et afficher la monnaie rendue
                decimal monnaieRendue = caisse.RendreMonnaie(montantPaiement1);

                // Générer et afficher le ticket de caisse
                caisse.GenererTicket(montantPaiement1, produitsAchetes, typePaiement1, ticket1);
            }
        }
    }
}
