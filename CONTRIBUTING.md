# Contribuer au projet

Nous sommes ravis que vous envisagiez de contribuer � notre projet ! Chaque contribution est importante et aide � am�liorer la communaut�.

## Comment contribuer ?

Les contributions peuvent prendre diff�rentes formes, telles que :

- Signaler des bugs
- Proposer de nouvelles fonctionnalit�s
- Am�liorer la documentation
- Soumettre des correctifs

## Signaler des bugs

Si vous trouvez un bug, merci de v�rifier si celui-ci a d�j� �t� signal�. Si ce n'est pas le cas, ouvrez une nouvelle issue en fournissant un titre et une description clairs, ainsi qu'une �tape par �tape pour reproduire le probl�me.

## Proposer des fonctionnalit�s

Si vous avez une id�e pour am�liorer le projet, ouvrez une issue pour en discuter avec les mainteneurs. Assurez-vous de donner autant de contexte que possible.

## Configuration du d�veloppement local

### Gestion des secrets d'application

Pour le d�veloppement local, nous utilisons l'outil Secret Manager pour stocker des informations sensibles telles que les secrets JWT en dehors de notre code de projet. Cela garantit que nos secrets sont conserv�s en s�curit� et ne sont pas accidentellement int�gr�s au contr�le de source.

#### Configuration du Secret Manager

Pour utiliser l'outil Secret Manager, suivez ces �tapes :

1. Naviguez jusqu'au r�pertoire du projet o� se trouve votre fichier `.csproj`.

2. Ex�cutez la commande suivante pour initialiser le Secret Manager pour votre projet :

```
dotnet user-secrets init
```

3. D�finissez la cha�ne de connexion dans le Secret Manager en ex�cutant :

```
dotnet user-secrets set "ConnectionStrings:IdentityDb" "Data Source=./Identity.db"
```

4. Ajoutez le secret JWT � vos secrets locaux en utilisant la commande suivante :

```
dotnet user-secrets set �JWT:Secret� �<Votre_Secret_JWT>�
```

Remplacez `<Votre_Secret_JWT>` par votre v�ritable secret JWT.

5. (Optionnel) D�finissez le `ValidAudience` et le `ValidIssuer` pour la configuration JWT :

```
dotnet user-secrets set �JWT:ValidAudience� �<Votre_Audience>�
dotnet user-secrets set �JWT:ValidIssuer� �<Votre_Emetteur>�
```

Remplacez `<Votre_Audience>` et `<Votre_Emetteur>` par les valeurs r�elles que vous souhaitez utiliser.

6. Pour v�rifier que les secrets ont �t� correctement d�finis, listez tous les secrets pour le projet :

```
dotnet user-secrets list
```

Les secrets sont stock�s de mani�re s�curis�e sur votre machine locale et sont associ�s � votre profil utilisateur.

## Pull Requests

Pour les pull requests, voici la proc�dure � suivre :

1. **Fork** le projet sur votre compte GitHub.
2. **Clone** le projet sur votre machine.
3. **Cr�ez une nouvelle branche** pour vos modifications.
4. **Faites vos modifications** et committez-les avec des messages clairs et explicatifs.
5. **Poussez** votre branche et ouvrez une pull request sur le projet original.
6. Assurez-vous que votre pull request passe tous les **tests** et **checks** mis en place.

## Code de conduite

Nous attendons de tous les contributeurs qu'ils respectent notre Code de Conduite. Par votre participation, vous acceptez de respecter ce code dans toutes vos interactions avec le projet.

## Besoin d'aide ?

Si vous avez besoin d'aide ou avez des questions, n'h�sitez pas � nous contacter.

Merci de contribuer � notre projet !

# Convention de Nommage des Commits

## Structure G�n�rale

Chaque message de commit doit �tre structur� comme suit :

```
<type>(<port�e>): <sujet> <corps du message> <pied de page>
```

## Description des �l�ments

- **type** : Indique le type de changement que vous apportez.
  - `feat` : Nouvelle fonctionnalit� pour l'utilisateur.
  - `fix` : Correction d'un bug pour l'utilisateur.
  - `docs` : Changements dans la documentation.
  - `style` : Changements qui n'affectent pas le sens du code (espaces, formatage, points-virgules manquants, etc).
  - `refactor` : Changement du code qui ne corrige ni ajoute une fonctionnalit�.
  - `perf` : Changement du code qui am�liore les performances.
  - `test` : Ajout ou correction de tests.
  - `chore` : Changements aux t�ches de build ou aux outils auxiliaires et biblioth�ques.

- **port�e** : La port�e du commit, par exemple `login`, `userModel`, `clientApp`, etc.

- **sujet** : Un bref r�sum� des changements.

- **corps du message** : Une description plus d�taill�e des changements.

- **pied de page** : R�f�rences aux num�ros d'issue ferm�s par ce commit.

## Exemples
```
feat(login): ajouter la v�rification du captcha Ajoute une v�rification captcha sur la page de connexion pour r�duire les tentatives de spam.

Closes #123
```

```
fix(server): corriger le crash lors de la r�initialisation du mot de passe Un crash se produisait lorsque l�email �tait null. Cela a �t� r�solu en ajoutant une v�rification pr�alable.

Closes #456
```
