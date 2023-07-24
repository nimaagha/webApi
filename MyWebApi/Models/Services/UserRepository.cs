using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;
using System;
using System.Linq;

namespace MyWebApi.Models.Services
{
    public class UserRepository
    {
        private readonly DatabaseContext context;
        public UserRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public User GetUser(Guid Id)
        {
            var user = context.users.SingleOrDefault(p => p.Id == Id);
            return user;
        }
        public bool ValidateUser(string Username, string Password)
        {
            var user = context.users.FirstOrDefault();
            return user != null ? true : false;
        }

        public string GetCode(string PhoneNumber)
        {
            Random random = new Random();
            string code = random.Next(1000, 9999).ToString();
            SmsCode smsCode = new SmsCode()
            {
                Code = code,
                InsertDate = DateTime.Now,
                PhoneNuber = PhoneNumber,
                RequestCount = 0,
                Used = false,
            };
            context.Add(smsCode);
            context.SaveChanges();
            return code;
        }
        public  LoginDto Login(string PhoneNumber, string Code)
        {
            var SmsCode = context.smsCodes.Where(p => p.PhoneNuber == PhoneNumber
            && p.Code == Code).FirstOrDefault();
            if(SmsCode == null)
            {
                return new LoginDto
                {
                    IsSuccess = false,
                    Message = "Inserted data is not correct",
                };
            }
            else
            {
                if(SmsCode.Used == true)
                {
                    return new LoginDto
                    {
                        IsSuccess = false,
                        Message = "Inserted data is not correct",
                    };
                }
                SmsCode.RequestCount++;
                SmsCode.Used = true;
                context.SaveChanges();
                var user = FindUserWithPhoneNumber(PhoneNumber);
                if(user != null)
                {
                    return new LoginDto
                    {
                        IsSuccess = true,
                        User = user,
                    };
                }
                else
                {
                    user = RegisterUser(PhoneNumber);
                    return new LoginDto
                    {
                        IsSuccess = true,
                        User = user,
                    };
                }
            }
        }

        public User FindUserWithPhoneNumber(string PhoneNumber)
        {
            var user = context.users.SingleOrDefault(p => p.PhoneNumber.Equals(PhoneNumber));
            return user;
        }
        public User RegisterUser(string PhoneNumber)
        {
            User user = new User()
            {
                PhoneNumber = PhoneNumber,
                IsActive = true,
            };
            return user;
        }
    }

    public class LoginDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
    }
}
