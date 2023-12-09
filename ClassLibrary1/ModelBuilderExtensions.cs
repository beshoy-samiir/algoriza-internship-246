using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Core.Domain.Lookup;
using Core.Domain.Users;

namespace Repository
{
    public static class ModelBuilderExtensions
    {
        public static void SeedRoles(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }, new Role
                {
                    Id = 2,
                    Name = "Doctor",
                    NormalizedName = "DOCTOR",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }, new Role
                {
                    Id = 3,
                    Name = "Patient",
                    NormalizedName = "PATIENT",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });
        }

        public static void SeedSpecializations(this ModelBuilder builder)
        {
            builder.Entity<Specialization>().HasData(
                new Specialization
                {
                    Id = 1,
                    Name = "Allergists",
                    Description = "They treat immune system disorders such as asthma, eczema, food allergies, insect sting allergies, and some autoimmune diseases."
                }, new Specialization
                {
                    Id = 2,
                    Name = "Anesthesiologists",
                    Description = "These doctors give you drugs to numb your pain or to put you under during surgery, childbirth, or other procedures. They monitor your vital signs while you’re under anesthesia."

                }, new Specialization
                {
                    Id = 3,
                    Name = "Cardiologists",
                    Description = "They’re experts on the heart and blood vessels. You might see them for heart failure, a heart attack, high blood pressure, or an irregular heartbeat."
                }, new Specialization
                {
                    Id = 4,
                    Name = "Dermatologists",
                    Description = "Have problems with your skin, hair, nails? Do you have moles, scars, acne, or skin allergies? Dermatologists can help."
                }, new Specialization
                {
                    Id = 5,
                    Name = "Neurologists",
                    Description = "These are specialists in the nervous system, which includes the brain, spinal cord, and nerves. They treat strokes, brain and spinal tumors, epilepsy, Parkinson's disease, and Alzheimer's disease."
                }, new Specialization
                {
                    Id = 6,
                    Name = "Pathologists",
                    Description = "These lab doctors identify the causes of diseases by examining body tissues and fluids under microscopes."
                }, new Specialization
                {
                    Id = 7,
                    Name = "Physiatrists",
                    Description = "These specialists in physical medicine and rehabilitation treat neck or back pain and sports or spinal cord injuries as well as other disabilities caused by accidents or diseases."
                }, new Specialization
                {
                    Id = 8,
                    Name = "Podiatrists",
                    Description = "They care for problems in your ankles and feet. That can include injuries from accidents or sports or from ongoing health conditions like diabetes."
                }, new Specialization
                {
                    Id = 9,
                    Name = "Radiologists",
                    Description = "They use X-rays, ultrasound, and other imaging tests to diagnose diseases."
                }, new Specialization
                {
                    Id = 10,
                    Name = "Urologists",
                    Description = "These are surgeons who care for men and women for problems in the urinary tract, like a leaky bladder."
                });
        }
    }
}
