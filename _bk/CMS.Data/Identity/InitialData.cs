using CMS.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMS.Data.Identity
{
    public static class InitialData
    {
        public static List<User> GetAdminUsers()
        {
            var users = new List<User>
            {
                //admin pass: abc123@ -> change later
                new User
                {
                    Id = "e7f18795-000c-4afe-995f-b71ec95fee30",
                    UserName = "Admin",
                    Name = "Admin",
                    LastName = "",
                    PasswordHash = "AF924q+3egq51yXasR/R3JYrcJAD7VzH48vt7ZzSyRf/5X+rC5TGvjoS4dUq6wNv4w==",
                    SecurityStamp = "3656ca8d-bcf0-4e6d-8d49-81e7da37fd6d"
                },
                new User
                {
                    Id = "061f64c8-1eba-44a8-ac70-04e8c129ee81",
                    UserName = "1",
                    Name = "User 1",
                    LastName = "",
                    PasswordHash = "AF924q+3egq51yXasR/R3JYrcJAD7VzH48vt7ZzSyRf/5X+rC5TGvjoS4dUq6wNv4w==",
                    SecurityStamp = "3656ca8d-bcf0-4e6d-8d49-81e7da37fd6d",
                }
            };
            return users;
        }

        public static List<IdentityRole> GetRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "f0ef553e-63bf-465d-b5f2-8abbe9768692",
                    Name = "Admin"
                },
                new IdentityRole
                {
                    Id = "08ae5d44-fb25-4ec7-b4e2-2aa62a8dfefc",
                    Name = "Operator"
                }
            };
        }

        public static List<IdentityUserRole> GetUsersInRoles()
        {
            var usersInRoles = new List<IdentityUserRole>();

            var adminUsers = GetAdminUsers();
            var adminRole = GetRoles().SingleOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                foreach (var adminUser in adminUsers)
                {
                    usersInRoles.Add(new IdentityUserRole
                    {
                        RoleId = adminRole.Id,
                        UserId = adminUser.Id
                    });
                }
            }

            return usersInRoles;
        }

        public static List<AccessPathCategory> GetAccessPathCategories()
        {
            return new List<AccessPathCategory>
            {
                new AccessPathCategory
                {
                    Id = Guid.Parse("ec00bcf7-dd13-4952-86dd-3617bcbee784"),
                    Title = "Home"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Employeement types management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Personnel groups management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Work units management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Positions management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Personnel management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Calendar management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("cfe54db0-b891-4f63-8606-257a32e02d5f"),
                    Title = "Shif on calendar management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Normal shifts management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Working hours management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Shift assignment management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Hourly shifts management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Hourly shifts assignment management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Duties management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Dismissals management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Duty requests management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title ="Dismissal requests management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Shift replacements management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Approval procedures management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Entrances management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Messages management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("b9e05fac-2879-4edb-92c4-02b53e1ae805"),
                    Title = "Role access paths management"
                },
                new AccessPathCategory
                {
                    Id=Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title="Roles management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("2e98edf9-6c14-442b-a788-320dbf7ecdf5"),
                    Title = "User info management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title="User management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("01705ce9-f696-4390-b092-870240b9a0fc"),
                    Title="Users and role management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Time access restrictions management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Restricted IPs management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("c9bffb79-9fe1-4884-9904-4fe24520933a"),
                    Title = "User logs management"
                },
                new AccessPathCategory
                {
                    Id = Guid.Parse("c672c807-1f62-4c2f-8e61-a195d48039dc"),
                    Title = "User profile management"
                },
            };
        }

        public static List<AccessPath> GetAccessPaths()
        {
            //cms
            var messagesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "List",
                    Path = "GET api/cms/messages",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Details",
                    Path = "GET api/cms/messages/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Create",
                    Path = "POST api/cms/messages/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Soft delete",
                    Path = "DELETE api/cms/messages/soft/{id}",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Permanent delete",
                    Path = "DELETE api/cms/messages/permanent/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("80027697-2857-4c81-aeba-6e8035a5fec9"),
                    Title = "Delete items",
                    Path = "DELETE api/cms/messages",
                    Priority = 5
                }
            };
            var restrictedAccessTimesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "List",
                    Path = "GET api/cms/restricted/access/times",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Details",
                    Path = "GET api/cms/restricted/access/times/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Create",
                    Path = "POST api/cms/restricted/access/times/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Update",
                    Path = "PUT api/cms/restricted/access/times/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Delete",
                    Path = "DELETE api/cms/restricted/access/times/permanent/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("367f75d6-d126-4130-8903-fb99147ae7dc"),
                    Title = "Delete items",
                    Path = "DELETE api/cms/restricted/access/times",
                    Priority = 6
                }
            };
            var restrictedIPsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "List",
                    Path = "GET api/cms/restricted/ips",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Details",
                    Path = "GET api/cms/restricted/ips/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Create",
                    Path = "POST api/cms/restricted/ips/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Update",
                    Path = "PUT api/cms/restricted/ips/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Delete",
                    Path = "DELETE api/cms/restricted/ips/permanent/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0df91819-eb69-420c-ae9a-463b6d1a0692"),
                    Title = "Delete items",
                    Path = "DELETE api/cms/restricted/ips",
                    Priority = 6
                }
            };
            var roleAccessPathsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9e05fac-2879-4edb-92c4-02b53e1ae805"),
                    Title = "Details",
                    Path = "GET api/cms/role/access/paths/{id}",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9e05fac-2879-4edb-92c4-02b53e1ae805"),
                    Title = "Update",
                    Path = "PUT api/cms/role/access/paths/update",
                    Priority = 1
                }
            };
            var rolesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title = "List",
                    Path = "GET api/cms/roles",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title = "Details",
                    Path = "GET api/cms/roles/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title = "Create",
                    Path = "POST api/cms/roles/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title = "Update",
                    Path = "PUT api/cms/roles/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("58e90c18-2c83-4194-89f8-97b9ea3026c3"),
                    Title = "Delete",
                    Path = "DELETE api/cms/roles/{id}",
                    Priority = 4
                }
            };
            var userInfoControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId=Guid.Parse("2e98edf9-6c14-442b-a788-320dbf7ecdf5"),
                    Title = "Details",
                    Path = "GET api/cms/user/info/{id}",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId=Guid.Parse("2e98edf9-6c14-442b-a788-320dbf7ecdf5"),
                    Title = "Update",
                    Path = "PUT api/cms/user/info/update",
                    Priority = 1
                }
            };
            var usersInRolesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("01705ce9-f696-4390-b092-870240b9a0fc"),
                    Title = "List",
                    Path = "GET api/cms/user/in/roles/{userId}",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("01705ce9-f696-4390-b092-870240b9a0fc"),
                    Title = "Details",
                    Path = "GET api/cms/user/in/roles/{userId}/{roleId}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("01705ce9-f696-4390-b092-870240b9a0fc"),
                    Title = "Create",
                    Path = "POST api/cms/user/in/roles/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("01705ce9-f696-4390-b092-870240b9a0fc"),
                    Title = "Delete",
                    Path = "DELETE api/cms/user/in/roles/{userId}/{roleId}",
                    Priority = 3
                }
            };
            var userLogsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId=Guid.Parse("c9bffb79-9fe1-4884-9904-4fe24520933a"),
                    Title = "List",
                    Path = "GET api/cms/user/logs/{userId}",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId=Guid.Parse("c9bffb79-9fe1-4884-9904-4fe24520933a"),
                    Title = "Details",
                    Path = "GET api/cms/user/logs/{id}",
                    Priority = 1
                }
            };
            var usersControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title = "List",
                    Path = "GET api/cms/users",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title = "Details",
                    Path = "GET api/cms/users/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title = "Create",
                    Path = "POST api/cms/users/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title = "Update",
                    Path = "PUT api/cms/users/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("e549df38-c585-479e-8ea8-81a3a11adb24"),
                    Title = "Delete",
                    Path = "DELETE api/cms/users/{id}",
                    Priority = 4
                }
            };
            //attendance management
            var employeementTypesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "List",
                    Path = "GET api/attendance/management/employeement/types",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Details",
                    Path = "GET api/attendance/management/employeement/types/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Create",
                    Path = "POST api/attendance/management/employeement/types/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/employeement/types/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/employeement/types/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/employeement/types/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/employeement/types/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c00981c0-10e3-494e-8376-3b62bae3d815"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/employeement/types",
                    Priority = 7
                }
            };
            var groupCategoriesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "List",
                    Path = "GET api/attendance/management/group/categories",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Details",
                    Path = "GET api/attendance/management/group/categories/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Create",
                    Path = "POST api/attendance/management/group/categories/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/group/categories/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/group/categories/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/group/categories/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/group/categories/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("9427f616-01e9-4284-ad1d-bb1f7f5b0f9d"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/group/categories",
                    Priority = 7
                }
            };
            var workUnitsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "List",
                    Path = "GET api/attendance/management/work/units",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Details",
                    Path = "GET api/attendance/management/work/units/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Create",
                    Path = "POST api/attendance/management/work/units/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/work/units/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/work/units/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/work/units/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/work/units/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2cd462f9-6801-4c67-a8d4-d6f74c4342cf"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/work/units",
                    Priority = 7
                }
            };
            var positionsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "List",
                    Path = "GET api/attendance/management/positions",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Details",
                    Path = "GET api/attendance/management/positions/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Create",
                    Path = "POST api/attendance/management/positions/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/positions/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/positions/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/positions/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/positions/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("405cc310-506d-440b-b038-7e12e84d2cef"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/positions",
                    Priority = 7
                }
            };
            var personnelControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/personnel/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("63a90aac-6fd2-43a2-8646-a3a4f06d7109"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel",
                    Priority = 7
                }
            };
            var shiftsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "List",
                    Path = "GET api/attendance/management/shifts",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Details",
                    Path = "GET api/attendance/management/shifts/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Create",
                    Path = "POST api/attendance/management/shifts/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/shifts/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/shifts/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/shifts/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/shifts/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("2d5831bf-cafa-4ff9-887e-b98ebf5ebb36"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/shifts",
                    Priority = 7
                }
            };
            var hourlyShiftsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "List",
                    Path = "GET api/attendance/management/hourly/shifts",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Details",
                    Path = "GET api/attendance/management/hourly/shifts/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Create",
                    Path = "POST api/attendance/management/hourly/shifts/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/hourly/shifts/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/hourly/shifts/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/hourly/shifts/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/hourly/shifts/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("3ef55801-ae83-48ec-b8d1-209fb68be2b8"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/hourly/shifts",
                    Priority = 7
                }
            };
            var workingHoursControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "List",
                    Path = "GET api/attendance/management/working/hours",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Details",
                    Path = "GET api/attendance/management/working/hours/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Create",
                    Path = "POST api/attendance/management/working/hours/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/working/hours/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/working/hours/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/working/hours/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/working/hours/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1baac4f6-8ba4-408d-bea8-b2c57276d6e2"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/working/hours",
                    Priority = 7
                }
            };
            var dutiesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "List",
                    Path = "GET api/attendance/management/duties",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Details",
                    Path = "GET api/attendance/management/duties/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Create",
                    Path = "POST api/attendance/management/duties/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/duties/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/duties/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/duties/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/duties/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("a493b993-99c6-4108-aec2-8ea6dabe670a"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/duties",
                    Priority = 7
                }
            };
            var dismissalSettingsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "List",
                    Path = "GET api/attendance/management/dismissal/settings/tabs",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "List (customized)",
                    Path = "GET api/attendance/management/dismissal/settings",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Details",
                    Path = "GET api/attendance/management/dismissal/settings/default/{dismissalType}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Details (cutomized)",
                    Path = "GET api/attendance/management/dismissal/settings/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Create",
                    Path = "POST api/attendance/management/dismissal/settings/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/dismissal/settings/update/default",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Update (customized)",
                    Path = "PUT api/attendance/management/dismissal/settings/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Partial update (customized)",
                    Path = "PATCH api/attendance/management/dismissal/settings/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Soft delete (customized)",
                    Path = "DELETE api/attendance/management/dismissal/settings/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Permanent delete (customized)",
                    Path = "DELETE api/attendance/management/dismissal/settings/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("d9696498-93f8-44cc-bff5-5777b7eea2cf"),
                    Title = "Delete items (customized)",
                    Path = "DELETE api/attendance/management/dismissal/settings",
                    Priority = 7
                }
            };
            var personnelDutiesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/duties",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/duties/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/duties/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/duties/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/duties/soft/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/duties/permanent/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/duties",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Accpet/reject",
                    Path = "POST api/attendance/management/personnel/duties/action",
                    Priority = 7
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("74cd4437-f80b-442e-be49-699eba74f6c6"),
                    Title = "Accept/reject items",
                    Path = "POST api/attendance/management/personnel/duties/action/list",
                    Priority = 8
                }
            };
            var personnelDismissalsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/dismissals",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/dismissals/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/dismissals/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/dismissals/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/dismissals/soft/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/dismissals/permanent/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/dismissals",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Accept/reject",
                    Path = "POST api/attendance/management/personnel/dismissals/action",
                    Priority = 7
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("610a6ff9-0683-4bc7-9796-2bdab37424f1"),
                    Title = "Accept/reject items",
                    Path = "POST api/attendance/management/personnel/dismissals/action/list",
                    Priority = 8
                }
            };
            var approvalProcsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "List",
                    Path = "GET api/attendance/management/approval/procs",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Details",
                    Path = "GET api/attendance/management/approval/procs/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Create",
                    Path = "POST api/attendance/management/approval/procs/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/approval/procs/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/group/categories/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/approval/procs/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/approval/procs/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("10a6cfea-aabc-416e-9877-29767ee699a2"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/approval/procs",
                    Priority = 7
                }
            };
            var calendarSettingsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "List",
                    Path = "GET api/attendance/management/calendar/settings",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Details",
                    Path = "GET api/attendance/management/calendar/settings/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Create",
                    Path = "POST api/attendance/management/calendar/settings/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/calendar/settings/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/calendar/settings/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("0f80ba70-d65f-4ffb-9d62-ba3188b57cc6"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/calendar/settings/permanent/{id}",
                    Priority = 6
                }
            };
            var personnelShiftAssignmentsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("cfe54db0-b891-4f63-8606-257a32e02d5f"),
                    Title = "View",
                    Path = "GET api/attendance/management/personnel/shift/assignments",
                    Priority = 0
                }
            };
            var personnelShiftsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/shifts",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/shifts/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/shifts/create/pattern",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/shifts/soft/{id}",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/shifts/permanent/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("6a0bf077-e820-4367-b25f-50f84caf94d1"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/shifts",
                    Priority = 5
                }
            };
            var personnelHourlyShiftsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/hourly/shifts",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/hourly/shifts/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/hourly/shifts/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/hourly/shifts/soft/{id}",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/hourly/shifts/permanent/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("23fad8a0-af83-4ea5-86b2-5ffc1e75f57c"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/hourly/shifts",
                    Priority = 5
                }
            };
            var personnelShiftReplacementsControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/shift/replacements",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/shift/replacements/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Create",
                    Path = "POST api/attendance/management/personnel/shift/replacements/create",
                    Priority = 2
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/shift/replacements/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/shift/replacements/soft/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/shift/replacements/permanent/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/shift/replacements",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Accept/reject",
                    Path = "POST api/attendance/management/personnel/shift/replacements/action",
                    Priority = 7
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("b9f638ef-6014-467f-b7c6-19c881a3fb93"),
                    Title = "Accept/reject items",
                    Path = "POST api/attendance/management/personnel/shift/replacements/action/list",
                    Priority = 8
                }
            };
            var personnelEntrancesControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "List",
                    Path = "GET api/attendance/management/personnel/entrances",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Details",
                    Path = "GET api/attendance/management/personnel/entrances/{id}",
                    Priority = 1
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/entrances/update",
                    Priority = 3
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Partial update",
                    Path = "PATCH api/attendance/management/personnel/entrances/update/{id}",
                    Priority = 4
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Soft delete",
                    Path = "DELETE api/attendance/management/personnel/entrances/soft/{id}",
                    Priority = 5
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Permanent delete",
                    Path = "DELETE api/attendance/management/personnel/entrances/permanent/{id}",
                    Priority = 6
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("1c0fe647-0909-498e-8564-8b5a48694716"),
                    Title = "Delete items",
                    Path = "DELETE api/attendance/management/personnel/entrances",
                    Priority = 7
                },
            };
            var homeControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.Parse("b6717c56-85f3-45f7-b226-124cb98ff4c6"),
                    ParentId = Guid.Parse("ec00bcf7-dd13-4952-86dd-3617bcbee784"),
                    Title = "Manager dashboard",
                    Path = "GET api/home/dashboard",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.Parse("f459d45f-4daa-45c7-84c4-aa90c0126d32"),
                    ParentId = Guid.Parse("ec00bcf7-dd13-4952-86dd-3617bcbee784"),
                    Title = "Personnel dashboard",
                    Path = "GET api/home/dashboard",
                    Priority = 0
                },
            };
            var personnelProfileControllerPaths = new List<AccessPath>
            {
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c672c807-1f62-4c2f-8e61-a195d48039dc"),
                    Title = "View",
                    Path = "GET api/attendance/management/personnel/profile",
                    Priority = 0
                },
                new AccessPath
                {
                    Id = Guid.NewGuid(),
                    ParentId = Guid.Parse("c672c807-1f62-4c2f-8e61-a195d48039dc"),
                    Title = "Update",
                    Path = "PUT api/attendance/management/personnel/profile/update",
                    Priority = 1
                },
            };

            var accessPathList = new List<AccessPath>();
            //cms
            accessPathList.AddRange(messagesControllerPaths);
            accessPathList.AddRange(restrictedAccessTimesControllerPaths);
            accessPathList.AddRange(restrictedIPsControllerPaths);
            accessPathList.AddRange(roleAccessPathsControllerPaths);
            accessPathList.AddRange(rolesControllerPaths);
            accessPathList.AddRange(userInfoControllerPaths);
            accessPathList.AddRange(usersInRolesControllerPaths);
            accessPathList.AddRange(userLogsControllerPaths);
            accessPathList.AddRange(usersControllerPaths);
            //attendance management
            accessPathList.AddRange(employeementTypesControllerPaths);
            accessPathList.AddRange(groupCategoriesControllerPaths);
            accessPathList.AddRange(workUnitsControllerPaths);
            accessPathList.AddRange(positionsControllerPaths);
            accessPathList.AddRange(personnelControllerPaths);
            accessPathList.AddRange(shiftsControllerPaths);
            accessPathList.AddRange(workingHoursControllerPaths);
            accessPathList.AddRange(dutiesControllerPaths);
            accessPathList.AddRange(dismissalSettingsControllerPaths);
            accessPathList.AddRange(personnelDutiesControllerPaths);
            accessPathList.AddRange(personnelDismissalsControllerPaths);
            accessPathList.AddRange(approvalProcsControllerPaths);
            accessPathList.AddRange(calendarSettingsControllerPaths);
            accessPathList.AddRange(personnelShiftAssignmentsControllerPaths);
            accessPathList.AddRange(personnelShiftsControllerPaths);
            accessPathList.AddRange(personnelShiftReplacementsControllerPaths);
            accessPathList.AddRange(hourlyShiftsControllerPaths);
            accessPathList.AddRange(personnelHourlyShiftsControllerPaths);
            accessPathList.AddRange(personnelEntrancesControllerPaths);
            accessPathList.AddRange(homeControllerPaths);
            accessPathList.AddRange(personnelProfileControllerPaths);

            return accessPathList;
        }

        public static List<RoleAccessPath> GetRolesAccessPaths(List<AccessPath> usedAccessPaths)
        {
            var roleAccessList = new List<RoleAccessPath>();

            var roles = GetRoles();
            var adminRole = roles.SingleOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                foreach (var accessPath in usedAccessPaths)
                {
                    roleAccessList.Add(new RoleAccessPath
                    {
                        RoleId = adminRole.Id,
                        AccessPathId = accessPath.Id
                    });
                }
            }

            return roleAccessList;
        }
    }
}