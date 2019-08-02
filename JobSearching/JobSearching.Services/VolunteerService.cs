using System;
using System.Linq;
using JobSearching.Services.Contracts;
using JobSearching.Services;
using System.Collections.Generic;
using System.Text;
using JobSearching.Data;
using JobSearching.ViewModels;
using JobSearching.Data.Models;

namespace JobSearching.Services
{
    public class VolunteerService : IVolunteerService
    {

        
        private JobSearchingDbContext context;
        public VolunteerService(JobSearchingDbContext context)
        {
            this.context = context;
        }
        
        private void Validate(string userName, string password, string firstName, string lastName, int age, string contact)
        {
            if(userName.Length < 6 || userName.Length > 20)
            {
                throw new ArgumentException("Please enter an username that has a length between 6 and 20 symbols.");
            }
            if(char.IsDigit(userName[0]))
            {
                throw new ArgumentException("Username cannot start with a digit. Please enter a different username.");
            }
            if(context.Volunteers.FirstOrDefault(v => v.Username == userName) != null)
            {
                throw new ArgumentException("The username entered is already taken. Please enter a different username.");
            }
            if (password.Length < 6 || password.Length > 20)
            {
                throw new ArgumentException("Please enter a password that has a length between 6 and 20 symbols.");
            }
            if (!firstName.All(char.IsLetter))
            {
                throw new ArgumentException("Please enter a first name that contains only letters.");
            }
            if (firstName.Length == 1 || firstName.Length > 15)
            {
                throw new ArgumentException("Please enter a first name with length between 2 and 15 letters.");
            }
            if (!char.IsUpper(firstName[0]) || !firstName.Skip(1).All(char.IsLower))
            {
                throw new ArgumentException("Please enter a first name that starts with an uppercase letter and follows with lowercase letters.");
            }
            if (!lastName.All(char.IsLetter))
            {
                throw new ArgumentException("Please enter a last name that contains only letters.");
            }
            if (lastName.Length == 1 || lastName.Length > 15)
            {
                throw new ArgumentException("Please enter a last name with length between 2 and 15 letters.");
            }
            if (!char.IsUpper(lastName[0]) || !lastName.Skip(1).All(char.IsLower))
            {
                throw new ArgumentException("Please enter a last name that starts with an uppercase letter and follows with lowercase letters.");
            }
            if (age < 16 || age > 100)
            {
                throw new ArgumentException("Please enter a valid age. You must be at least 16 years old to use this site.");
            }
        }

        public int CreateVolunteer(string userName, string password, string firstName, string lastName, int age, string contact)
        {

            Validate(userName, password, firstName, lastName, age, contact);
             
            var newVolunter = new Volunteer()
            {
                Username = userName,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                ContactInformation = contact
            };
            
            context.Volunteers.Add(newVolunter);

            context.SaveChanges();

            context.Entry(newVolunter).GetDatabaseValues();
            //secures that object newVolunteer will have properly Id

            return newVolunter.Id;

        }
        

        public int LogInVolunteer(string userName, string password)
        {
            // При успешен logIn актуализирай CurrentSigned.VolunteerId
            Volunteer volunteer = context.Volunteers.FirstOrDefault(v => v.Username == userName);
            if(volunteer != null)
            {
                if(volunteer.Password == password)
                {
                    CurrentSigned.VolunteerId = volunteer.Id;
                    return volunteer.Id;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        
        public bool ChangeVolunteer(string userName, string oldPassword, string newPassword, string firstName, string lastName, int age, string contact)
        {
            if (LogInVolunteer(userName, oldPassword) == -1)
            {
                return false;
            }
            else
            {
                if (newPassword == null) newPassword = oldPassword;
                Validate(userName, newPassword, firstName, lastName, age, contact);

                var vol = context.Volunteers.First(a => a.Id == CurrentSigned.VolunteerId);

                vol.Age = age;
                vol.ContactInformation = contact;
                vol.FirstName = firstName;
                vol.LastName = lastName;
                vol.Password = newPassword;
                context.Update(vol);
                context.SaveChanges();
                
            }
            return true;
        }


        public VolunteerDetailViewModel GetVolunteer(int id)
        {
            var volunteer = context.Volunteers.Find(id);

            if (volunteer == null) throw new IndexOutOfRangeException();

            var model = new VolunteerDetailViewModel()
            {
                Username = volunteer.Username,
                FirstName = volunteer.FirstName,
                LastName = volunteer.LastName,
                Age = volunteer.Age,
                Contact = volunteer.ContactInformation
            };

            return model;

        }


        public VolunteerProfileViewModel GetSignedVolunteer()
        {
            if (CurrentSigned.VolunteerId == -1) throw new ArgumentException("You need to be Logged In");

            var volunteer = context.Volunteers.Find(CurrentSigned.VolunteerId);
            var ads = new List<AdvertSingleViewModel>();
            
            
            var model = new VolunteerProfileViewModel()
            {
                Username = volunteer.Username,
                FirstName = volunteer.FirstName,
                LastName = volunteer.LastName,
                Age = volunteer.Age,
                Contact = volunteer.ContactInformation,
                SignedInAds = new IndexSingleAdViewModel() { Ads = ads }

            };

            return model;
        }

        
    }
}
