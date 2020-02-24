using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.InteropServices;

namespace ADMate {
    public class AdMate {
        private readonly PrincipalContext _pc;

        public AdMate(string domain, string username, string password) {
            _pc = new PrincipalContext(ContextType.Domain, domain, username, password);
            // if (!pc.ValidateCredentials(username, password)) {
            //     throw new ValidationException("Incorrect Username or Password");
            // }
        }

        public bool AddUserToGroup(string userId, string groupName) {
            try {
                UserPrincipal user = UserPrincipal.FindByIdentity(_pc, userId);
                GroupPrincipal group = GroupPrincipal.FindByIdentity(_pc, groupName);

                if (user != null && group != null && !group.Members.Contains(user)) {
                    group.Members.Add(user);
                    group.Save();
                    return true;
                }

                return false;
            } catch (COMException e) {
                throw e;
            }
        }

        public bool RemoveUserFromGroup(string userId, string groupName) {
            try {
                UserPrincipal user = UserPrincipal.FindByIdentity(_pc, userId);
                GroupPrincipal group = GroupPrincipal.FindByIdentity(_pc, groupName);

                if (user != null && group != null && group.Members.Contains(user)) {
                    group.Members.Remove(user);
                    group.Save();
                    return true;
                }

                return false;
            } catch (COMException e) {
                throw e;
            }
        }

        public List<string> GetGroupUsers(string groupName) {
            GroupPrincipal group = GroupPrincipal.FindByIdentity(_pc, groupName);

            List<string> result = new List<string>();
            foreach (var member in group.GetMembers()) {
                result.Add(member.Name);
            }
            return result;
        }

        public List<string> GetUserGroups(string userId) {
            try {
                UserPrincipal user = UserPrincipal.FindByIdentity(_pc, userId);


                List<string> result = new List<string>();
                foreach (var group in user.GetGroups()) {
                    result.Add(group.Name);
                }

                return result;
            } catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}