# Identity API

Cet API permet de gérer les identités utilisateurs, l'authentification et l'inscription. Voici comment l'utiliser:

## Prérequis
- .NET SDK (version 8 ou ultérieure)
- Visual Studio ou Visual Studio Code (facultatif)

## Utilisation
1. Inscrivez-vous en utilisant l'API `/api/v1/register` avec une requête POST contenant l'adresse e-mail et le mot de passe.
2. Connectez-vous en utilisant l'API `/api/v1/auth` avec une requête POST contenant l'adresse e-mail et le mot de passe.

## Personnalisation
- Vous pouvez personnaliser les modèles d'inscription et de connexion en ajoutant des champs supplémentaires (nom, prénom, etc.).
- Étendez les contrôleurs pour gérer les rôles et les autorisations.

## Licence
Ce projet est sous licence MIT. Consultez le fichier `LICENSE` pour plus d'informations.
