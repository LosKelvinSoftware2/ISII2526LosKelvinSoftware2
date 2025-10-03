#[Instrucciones en Español para Grupo A y Grupo B](#instrucciones-para-utilizar-esta-plantilla).

#[Instructions in English for Group I](#instructions-to-use-this-template).

# Instrucciones para utilizar esta plantilla.

## 1. Cree la organización 

- Cree la organización para su equipo con el nombre **ISII-2526-GrupoX-Team**, donde GrupoX puede ser GrupoA o GrupoB siguiendo las instrucciones disponibles en:  
[Create an organization](https://docs.github.com/en/organizations/collaborating-with-groups-in-organizations/creating-a-new-organization-from-scratch).

- Establezca que dicha organización pertenece a su **cuenta personal**. 


### 1.1 Añada un equipo a su organización

Siga las instrucciones para crear el equipo [Creating an organization team](https://docs.github.com/en/organizations/organizing-members-into-teams/creating-a-team)


### 1.2 Añada miembros al equipo de su organización

Siga las instrucciones para añadir miembros [Adding organization members to a team](https://docs.github.com/en/organizations/organizing-members-into-teams/adding-organization-members-to-a-team)


## 2. Crear el proyecto

Cree su proyecto con el nombre ISII2526TeamName utilizando la plantilla disponible en: [Plantilla](https://github.com/orgs/Ingenieria-del-Software-II-ESIIAB/projects/14/views/1)

- ADVERTENCIA: el nombre del proyecto NO PUEDE CONTENER ESPACIOS NI CARACTERES NO ALFABÉTICOS
- Establezca que **el propietario es la organización que acabas de crear.**
- Establezca la visibilidad (visibility) **como pública**. [establece la visibilidad](https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-your-project/managing-visibility-of-your-projects#changing-project-visibility)


## 3. Cree el repositorio

Cree el repositorio de su equipo con el nombre **ISII2526TeamName** utilizando esta plantilla: [Plantilla](https://github.com/Ingenieria-del-Software-II-ESIIAB/SEII-IPO-template)
- Siga las instrucciones disponibles en:  
[Create a repository from a template](https://docs.github.com/es/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template#creating-a-repository-from-a-template).
- Establezca que el **propietario (owner) es la organización que acaba de crear**.
- Establezca la visibilidad (visibility) **como pública**.
- Enlace el repositorio con el proyecto creado antes [Adding your project to a repository](https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-your-project/adding-your-project-to-a-repository)

## 4. Configure el repositorio de su equipo.

### 4.1. En la rama development modificar el archivo `info.yml`.

```yaml
project:
  name: 'ISII-2526-GrupoX-Team'
  owner: 'GrupoX'
  teamId: 'Team'
  identities: {}
  notifications:
    email: 'member1@alu.uclm.es,member2@alu.uclm.es,member3@alu.uclm.es,member4@alu.uclm.es'
  members:
    member1:
      name: 'Name1'
      surname: 'Surname1' 
      githubUsername: 'Name1gitUserName1'
    member2:
      name: 'Name2'
      surname: 'Surname2' 
      githubUsername: 'Name1gitUserName2'
    member3:
      name: 'Name3'
      surname: 'Surname3' 
      githubUsername: 'Name1gitUserName3'
    member4:
      name: 'Name4'
      surname: 'Surname4' 
      githubUsername: 'Name1gitUserName4'
```

- Donde, GrupoX puede ser GrupoA, GrupoB, o GrupoI, y Team es el nombre del equipo
    > Por ejemplo, el equipo "TheLeaders" matriculado en el GrupoI sería:
    >```yaml
    >name: 'ISII-2526-GrupoI-TheLeaders'
    >owner: 'GrupoI'
    >teamId: 'TheLeaders'
    >```

- Sustituir los datos de cada memberN por los del miembro real.
- Si el número de miembros es 3 entonces, eliminar member4 de la sección `members`.
- Modificar la cadena de `notifications.email` para que contengan sólo los correos @alu.uclm.es de todos los miembros separados por comas y sin espacios.
- Los githubUsername tienen que existir en GitHub.

### 4.2. Unir el proyecto a la asignatura auditada por Bluejay.
- Cree la rama **main** utilizando development como fuente [como crear una rama](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-and-deleting-branches-within-your-repository#creating-a-branch-via-the-branches-overview)
- Accede a [join.bluejay.governify.io](https://join.bluejay.governify.io).
- Añade la **URL del repositorio** de GitHub. Por ejemplo, la url de este repositorio es "https://github.com/Ingenieria-del-Software-II-ESIIAB/SEII-IPO-template"
- Click en **CHECK**. Si ha dado error revisa la [sintaxis](https://www.yamllint.com/) del info.yml.
- **Selecciona el curso** al que te quieres unir (UCLM-ISII-2526) y especifica el código que te dará tu profesor.
- Click en **JOIN**.
- En caso de que haya algún otro problema al configurar Bluejay, el equipo de soporte está disponible en [un canal de Gitter dedicado](https://app.gitter.im/#/room/!VTAnLfNgxrEdydQgWd:gitter.im).


### 4.3. Añada los miembros de su equipo al repo.
- Invite al Equipo que ha creado a su repositorio con el Role **Write**: [Invitar a un equipo](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/managing-teams-and-people-with-access-to-your-repository#inviting-a-team-or-person)




# Instructions to use this template.

## 1. Create the organisation

Create your team's organisation with the name **ISII-2526-GrupoI-Team** where Team is the name of your team:

- Follow the instructions available at 
[Create an organisation](https://docs.github.com/en/organizations/collaborating-with-groups-in-organizations/creating-a-new-organization-from-scratch).

- Establish that this organisation **belongs to your personal account**


**1.1 Add a team to your organisation**

Follow the instructions to create the team [Creating an organisation team](https://docs.github.com/en/organizations/organizing-members-into-teams/creating-a-team)

**1.2 Add members to your organisation team**

Follow the instructions to add members [Adding organisation members to a team](https://docs.github.com/en/organizations/organizing-members-into-teams/adding-organization-members-to-a-team)

## 2. Create the Project

Create your project with the name ISII2526TeamName using the template available at: [Template](https://github.com/orgs/Ingenieria-del-Software-II-ESIIAB/projects/14/views/1)

- WARNING: the project name CANNOT CONTAIN SPACES OR NON-ALPHABETICAL CHARACTERS
- Establish that **the owner is the organisation you have just created.**
- Establish the visibility of the project as **public**. [set visibility](https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-your-project/managing-visibility-of-your-projects#changing-project-visibility)

## 3. Create the repository

Create your team's repository with the name **ISII2526TeamName**, using this template: [Template](https://github.com/Ingenieria-del-Software-II-ESIIAB/SEII-IPO-template).

- Follow the instructions available at:  
[Create a repository from a template](https://docs.github.com/es/repositories/creating-and-managing-repositories/creating-a-repository-from-a-template#creating-a-repository-from-a-template).
- Establish that **the owner is the organisation you have just created.**
- Establish that **Visibility is Public.**
- Link the repo with the project you have just created [Adding your project to a repository](https://docs.github.com/en/issues/planning-and-tracking-with-projects/managing-your-project/adding-your-project-to-a-repository)

## 4. Configure your team's repository.

### 4.1. In the development branch, modify the `info.yml` file.

```yaml
project:
  name: 'ISII-2526-GrupoX-Team'
  owner: 'GrupoX'
  teamId: 'Team'
  identities: {}
  notifications:
    email: 'member1@alu.uclm.es,member2@alu.uclm.es,member3@alu.uclm.es,member4@alu.uclm.es'
  members:
    member1:
      name: 'Name1'
      surname: 'Surname1' 
      githubUsername: 'Name1gitUserName1'
    member2:
      name: 'Name2'
      surname: 'Surname2' 
      githubUsername: 'Name1gitUserName2'
    member3:
      name: 'Name3'
      surname: 'Surname3' 
      githubUsername: 'Name1gitUserName3'
    member4:
      name: 'Name4'
      surname: 'Surname4' 
      githubUsername: 'Name1gitUserName4'
```

- Where GroupX must be GroupI, and Team is the name of the team
    > For example, the team ‘TheLeaders’ registered in GroupI would be:
    >```yaml
    >name: 'ISII-2526-GrupoI-TheLeaders'
    >owner: 'GroupI'
    >teamId: 'TheLeaders'
    >```
- Replace the data for each memberN with that of the actual member.
- If the number of members is 3, delete member4 from the `members` section.
- Modify the `notifications:email` string so that it only contains the @alu.uclm.es email addresses of all members, separated by commas and without spaces.

### 4.2. Link the project to the course audited by Bluejay
- Create the **main** branch using development as the source [How to create a branch](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-and-deleting-branches-within-your-repository#creating-a-branch-via-the-branches-overview)
- Go to [join.bluejay.governify.io](https://join.bluejay.governify.io).
- Add the GitHub URL of **your repository**. For instance, the URL of this repo is "https://github.com/Ingenieria-del-Software-II-ESIIAB/SEII-IPO-template"
- Click **CHECK**. If you get an error, check the [syntax](https://www.yamllint.com/) of the info.yml.
- **Select the course** you want to join (UCLM-ISII-2526) and specify the password your teacher will give you.
- Click **JOIN**.
- If you encounter any other problems when configuring Bluejay, the support team is available on a dedicated Gitter channel (https://app.gitter.im/#/room/!VTAnLfNgxrEdydQgWd:gitter.im).


### 4.3. Add your team to the repo.

Invite the Team you have created with the Role **Write**:[Inviting a team](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/managing-teams-and-people-with-access-to-your-repository#inviting-a-team-or-person)
