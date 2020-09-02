using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App
{
    public static class SeedDataDb
    {
        public static void SeedDataContext(this BookDBContext context)
        {
            var booksAuthors = new List<BookAuthor>()
            {
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "123",
                        Title = "The Call Of The Wild",
                        PublishedDate = new DateTime(1903,1,1),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Action"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Awesome Book", ReviewText = "Reviewing Call of the Wild and it is awesome beyond words", Rating = 5,
                             Reviewers = new Reviewer(){ FirstName = "John", LastName = "Smith" } },
                            new Review { HeadLine = "Terrible Book", ReviewText = "Reviewing Call of the Wild and it is terrrible book", Rating = 1,
                             Reviewers = new Reviewer(){ FirstName = "Peter", LastName = "Griffin" } },
                            new Review { HeadLine = "Decent Book", ReviewText = "Not a bad read, I kind of liked it", Rating = 3,
                             Reviewers = new Reviewer(){ FirstName = "Paul", LastName = "Griffin" } }
                        }
                    },
                    Author = new Author()
                    {
                        First_Name = "Jack",
                        Last_Name = "London",
                        Country = new Country()
                        {
                            Name = "USA"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "1234",
                        Title = "Winnetou",
                        PublishedDate = new DateTime(1878,10,1),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Western"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Awesome Western Book", ReviewText = "Reviewing Winnetou and it is awesome book", Rating = 4,
                             Reviewers = new Reviewer(){ FirstName = "Frank", LastName = "Gnocci" } }
                        }
                    },
                    Author = new Author()
                    {
                        First_Name = "Karl",
                        Last_Name = "May",
                        Country = new Country()
                        {
                            Name = "Germany"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "12345",
                        Title = "Pavols Best Book",
                        PublishedDate = new DateTime(2019,2,2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Educational"}},
                            new BookCategory { Category = new Category() { Name = "Computer Programming"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Awesome Programming Book", ReviewText = "Reviewing Pavols Best Book and it is awesome beyond words", Rating = 5,
                             Reviewers = new Reviewer(){ FirstName = "Pavol2", LastName = "Almasi2" } }
                        }
                    },
                    Author = new Author()
                    {
                        First_Name = "Pavol",
                        Last_Name = "Almasi",
                        Country = new Country()
                        {
                            Name = "Slovakia"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "123456",
                        Title = "Three Musketeers",
                        PublishedDate = new DateTime(2019,2,2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Action"}},
                            new BookCategory { Category = new Category() { Name = "History"}}
                        }
                    },
                    Author = new Author()
                    {
                        First_Name = "Alexander",
                        Last_Name = "Dumas",
                        Country = new Country()
                        {
                            Name = "France"
                        }
                    }
                },
                new BookAuthor()
                {
                    Book = new Book()
                    {
                        Isbn = "1234567",
                        Title = "Big Romantic Book",
                        PublishedDate = new DateTime(1879,3,2),
                        BookCategories = new List<BookCategory>()
                        {
                            new BookCategory { Category = new Category() { Name = "Romance"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review { HeadLine = "Good Romantic Book", ReviewText = "This book made me cry a few times", Rating = 5,
                             Reviewers = new Reviewer(){ FirstName = "Allison", LastName = "Kutz" } },
                            new Review { HeadLine = "Horrible Romantic Book", ReviewText = "My wife made me read it and I hated it", Rating = 1,
                             Reviewers = new Reviewer(){ FirstName = "Kyle", LastName = "Kutz" } }
                        }
                    },
                    Author = new Author()
                    {
                        First_Name = "Anita",
                        Last_Name = "Powers",
                        Country = new Country()
                        {
                            Name = "Canada"
                        }
                    }
                }
            };

            context.BookAuthors.AddRange(booksAuthors);
            context.SaveChanges();
        }
    }
}
