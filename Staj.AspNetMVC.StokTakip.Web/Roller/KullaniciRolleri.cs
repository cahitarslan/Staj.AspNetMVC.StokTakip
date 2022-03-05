using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Staj.AspNetMVC.StokTakip.Web.Models.Entities;

namespace Staj.AspNetMVC.StokTakip.Web.Roller
{
    public class KullaniciRolleri : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }


        private readonly StokTakipDbEntities entity = new StokTakipDbEntities();
        public override string[] GetRolesForUser(string username)
        {
            //var kullanici = entity.Kullanicilar.FirstOrDefault(I => I.KullaniciAdi == username);
            //return new string[] { kullanici.Rol };
            List<KullaniciRoller> kullaniciRoller = entity.KullaniciRoller.Where(I => I.Kullanicilar.KullaniciAdi == username).ToList();
            string[] roller = new string[kullaniciRoller.Count];
            if (kullaniciRoller.Count > 0)
            {
                for (int i = 0; i < roller.Length; i++)
                {
                    foreach (var item in kullaniciRoller)
                    {
                        roller[i] = item.Roller.RolAdi.Trim();
                        kullaniciRoller.Remove(item);
                        break;
                    }
                }
                return roller;
            }
            return new string[] { "" };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}