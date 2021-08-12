using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserInfo;

namespace Asp_Xioma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfosController : ControllerBase
    {
        
        [HttpGet("pull")]
        public List<UserInf> Pull()
        {
            return DB.PullData<UserInf>(
                "SELECT Id, FirstName, LastName, DateOfBirth,Role,Department, DateOfEntry, Remarks,Address FROM UserInfo",
                (dr) => new UserInf
                {
                    Id=dr.IsDBNull(0)?0:dr.GetInt32(0),
                    FirstName=dr.GetStringOrNull(1),
                    LastName=dr.GetStringOrNull(2),
                    DateOfBirth=dr.IsDBNull(3)? DateTime.MinValue : dr.GetDateTime(3),
                    Role =dr.GetStringOrNull(4),
                    Department=dr.GetStringOrNull(5),
                    DateOfEntry=dr.IsDBNull(6)? DateTime.MinValue : dr.GetDateTime(6),
                    Remarks=dr.GetStringOrNull(7),
                    Address=dr.GetStringOrNull(8)

                });
        }


        
        [HttpPost("insert")]
        public bool Insert(UserInf user)
        {
            return DB.Modify("INSERT INTO UserInfo (Id,FirstName, LastName,DateOfBirth,Role,Department,DateOfEntry,Remarks,Address) VALUES (@Id,@FirstName, @LastName,@DateOfBirth,@Role,@Department,@DateOfEntry,@Remarks,@Address)",
                (cmd) => {
                    cmd
                    .AddWithValue("@Id", user.Id)
                    .AddWithValue("@FirstName", user.FirstName)
                    .AddWithValue("@LastName", user.LastName)
                    .AddWithValue("@DateOfBirth", user.DateOfBirth)
                    .AddWithValue("@Role", user.Role)
                    .AddWithValue("@Department", user.Department)
                    .AddWithValue("@DateOfEntry", user.DateOfEntry)
                    .AddWithValue("@Remarks", user.Remarks)
                    .AddWithValue("@Address", user.Address);

                }) == 1;
        }




      
        [HttpPost("delete")]
        public bool Delete(UserInf user)
        {
            return DB.Modify("DELETE FROM UserInfo WHERE Id=@Id", (cmd) => cmd.AddWithValue("@Id", user.Id)) == 1;
        }



       
        [HttpPost("update")]
        public bool Update(UserInf user)
        {
            return DB.Modify("UPDATE UserInfo SET FirstName=@FirstName, LastName=@LastName, DateOfBirth=@DateOfBirth, Role=@Role, Department=@Department ,DateOfEntry=@DateOfEntry ,Remarks=@Remarks,Address=@Address WHERE Id = @Id",
                (cmd) => {
                    cmd
                    .AddWithValue("@FirstName", user.FirstName)
                    .AddWithValue("@LastName", user.LastName)
                    .AddWithValue("@DateOfBirth", user.DateOfBirth)
                    .AddWithValue("@Role", user.Role)
                    .AddWithValue("@Department", user.Department)
                    .AddWithValue("@DateOfEntry", user.DateOfEntry)
                    .AddWithValue("@Remarks", user.Remarks)
                    .AddWithValue("@Address", user.Address)
                    .AddWithValue("@Id", user.Id);

                }) == 1;
        }




    }


    //Extention method 
    public static class MyExtensionMethod
    {
        public static SqlCommand AddWithValue(this SqlCommand cmd, string parameterName, object value)
        {
            if (value == null)
                cmd.Parameters.AddWithValue(parameterName, DBNull.Value);
            else
                cmd.Parameters.AddWithValue(parameterName, value);
            return cmd;
        }

        public static string GetStringOrNull(this SqlDataReader dr, int i)
        {
            return dr.IsDBNull(i) ? null : dr.GetString(i);
        }



    }
}
