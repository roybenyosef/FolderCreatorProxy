using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Security.AccessControl;
using System.IO;
using FolderCreatorInterfaces;

namespace FolderCreatorProxy
{
    class FolderCreator : IFolderCreator
    {
        public FolderCreationResult CreateFolder(string username)
        {
            try
            {
                var domainName = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain().Name;
                var targetFolder = $@"\\filer\users$\{username}";

                ValidateUserExists(username);
                ValidateFolderExsits(targetFolder);
                CreateFolderOnDisk(targetFolder);
                SetUserPermissionsOnFolder(targetFolder, username);

                return CreateOkResult(targetFolder, username);
            }
            catch (Exception ex)
            {
                return CreateErrorResult(ex.Message);
            }
        }

        private void SetUserPermissionsOnFolder(string targetFolder, string username)
        {
            if (!SetAcl(targetFolder, username))
            {
                throw new Exception($"Error setting acl on folder '{targetFolder}', for user '{username}'");
            }
        }

        private void CreateFolderOnDisk(string targetFolder)
        {
            System.IO.Directory.CreateDirectory(targetFolder);
        }

        private void ValidateFolderExsits(string targetFolder)
        {
            if (System.IO.Directory.Exists(targetFolder))
            {
                throw new MissingMemberException($"Folder {targetFolder} already exist!");
            }
        }

        private void ValidateUserExists(string username)
        {
            if (UserExist(username))
            {
                throw new MissingMemberException($"User '{username}' could not be found");
            }
        }

        bool UserExist(string username)
        {
            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(
                    principalContext, IdentityType.SamAccountName, username);

                return userPrincipal != null;
            }
        }

        static bool SetAcl(string destinationDirectory, string username)
        {
            FileSystemRights Rights = (FileSystemRights)0;
            Rights = FileSystemRights.FullControl;

            // *** Add Access Rule to the actual directory itself
            FileSystemAccessRule AccessRule = new FileSystemAccessRule(username, Rights,
                                        InheritanceFlags.None,
                                        PropagationFlags.NoPropagateInherit,
                                        AccessControlType.Allow);

            DirectoryInfo Info = new DirectoryInfo(destinationDirectory);
            DirectorySecurity Security = Info.GetAccessControl(AccessControlSections.Access);

            bool Result = false;
            Security.ModifyAccessRule(AccessControlModification.Set, AccessRule, out Result);

            if (!Result)
                throw new Exception("ModifyAccessRule failed to update the access rule with 'set' access control");

            // *** Always allow objects to inherit on a directory
            InheritanceFlags iFlags = InheritanceFlags.ObjectInherit;
            iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

            // *** Add Access rule for the inheritance
            AccessRule = new FileSystemAccessRule(username, Rights,
                                        iFlags,
                                        PropagationFlags.InheritOnly,
                                        AccessControlType.Allow);
            Result = false;
            Security.ModifyAccessRule(AccessControlModification.Add, AccessRule, out Result);

            if (!Result)
                throw new Exception("ModifyAccessRule failed to update the access rule with 'add' access control");

            Info.SetAccessControl(Security);

            return true;
        }

        private FolderCreationResult CreateErrorResult(string errorMessage)
        {
            return new FolderCreationResult
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }

        private FolderCreationResult CreateOkResult(string targetFolder, string username)
        {
            return new FolderCreationResult
            {
                Success = true,
                ErrorMessage = string.Empty
            };
        }

    }
}
