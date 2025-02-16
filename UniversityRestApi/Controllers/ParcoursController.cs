﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Dtos;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.ParcoursUseCases.Delete;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.Get;
using UniversiteDomain.UseCases.SecurityUseCases.Get;

namespace UniversityRestApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    
    public class ParcoursController (IRepositoryFactory repositoryFactory): ControllerBase
    {
        [HttpGet("complet/{id}")]
        public async Task<ActionResult<ParcoursDto>> GetParcoursAsync(long id)
        {
            // Identification et authentification
            string role="";
            string email="";
            IUniversiteUser user = null;
            try
            {
                CheckSecu(out role, out email, out user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
    
            GetParcoursUseCase uc = new GetParcoursUseCase(repositoryFactory);
            // Autorisation
            // On vérifie si l'utilisateur connecté a le droit d'accéder à la ressource
            if (!uc.IsAuthorized(role, user, id)) return Unauthorized();
            Parcours? parcours;
            try
            {
                parcours = await uc.ExecuteAsync(id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(e), e.Message);
                return ValidationProblem();
            }
            if (parcours== null) return NotFound();
            return new ParcoursDto().ToDto(parcours);
        }

        [HttpPost]
        public async Task<ActionResult<ParcoursDto>> PostAsync([FromBody] ParcoursDto parcoursDto)
        {
           CreateParcoursUseCase createParcoursUc = new CreateParcoursUseCase(repositoryFactory);
           string role="";
           string email="";
           IUniversiteUser user = null;
           CheckSecu(out role, out email, out user);
           
           if (!createParcoursUc.IsAuthorized(role) || !createParcoursUc.IsAuthorized(role)) return Unauthorized();

           Parcours parcours = parcoursDto.ToEntity();
           
           try
           {
               parcours = await createParcoursUc.ExecuteAsync(parcours);
           }
           catch (Exception e)
           {
               ModelState.AddModelError(nameof(e), e.Message);
               return ValidationProblem();
           }
            
           ParcoursDto dto = new ParcoursDto().ToDto(parcours);
           return CreatedAtAction(nameof(GetUnParcours), new { id = dto.Id }, dto);
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private void CheckSecu(out string role, out string email, out IUniversiteUser user)
        {
            role = "";
            // Récupération des informations de connexion dans la requête http entrante
            ClaimsPrincipal claims = HttpContext.User;
            // Faisons nos tests pour savoir si la personne est bien connectée
            if (claims.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException();
            // Récupérons le email de la personne connectée
            if (claims.FindFirst(ClaimTypes.Email)==null) throw new UnauthorizedAccessException();
            email = claims.FindFirst(ClaimTypes.Email).Value;
            if (email==null) throw new UnauthorizedAccessException();
            // Vérifions qu'il est bien associé à un utilisateur référencé
            user = new FindUniversiteUserByEmailUseCase(repositoryFactory).ExecuteAsync(email).Result;
            if (user==null) throw new UnauthorizedAccessException();
            // Vérifions qu'un rôle a bien été défini
            if (claims.FindFirst(ClaimTypes.Role)==null) throw new UnauthorizedAccessException();
            // Récupérons le rôle de l'utilisateur
            var ident = claims.Identities.FirstOrDefault();
            if (ident == null)throw new UnauthorizedAccessException();
            role = ident.FindFirst(ClaimTypes.Role).Value;
            if (role == null) throw new UnauthorizedAccessException();
            // Vérifions que le user a bien le role envoyé via http
            bool isInRole = new IsInRoleUseCase(repositoryFactory).ExecuteAsync(email, role).Result; 
            if (!isInRole) throw new UnauthorizedAccessException();
            // Si tout est passé sans renvoyer d'exception, le user est authentifié et conncté
        }
    }
}