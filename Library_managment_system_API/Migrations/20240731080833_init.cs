using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library_managment_system_API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Catagory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubCatagory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Ordered = table.Column<bool>(type: "bit", nullable: false),
                    BookCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_BookCategories_BookCategoryId",
                        column: x => x.BookCategoryId,
                        principalTable: "BookCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Returned = table.Column<bool>(type: "bit", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinePaid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BookCategories",
                columns: new[] { "Id", "Catagory", "SubCatagory" },
                values: new object[,]
                {
                    { 1, "Computer", "DataStructure" },
                    { 2, "Mechanical", "Machine" },
                    { 3, "Methamatics", "Calculas" },
                    { 4, "Methamatics", "Algebra" },
                    { 5, "Computer", "C# Programing" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountStatus", "CreatedOn", "Email", "FirstName", "LastName", "MobileNumber", "Password", "UserType" },
                values: new object[] { 1, "ACTIVE", new DateTime(2023, 12, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "mehedi@gmail.com", "Mehedi", "kazi", "01750323645", "Password1234", "ADMIN" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "BookCategoryId", "Ordered", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Thomas H. Cormen", 1, false, 50000.0, "Author Thomas Cormen’s “Algorithms Unlocked” seeks to take away the mystery of technology and unveils the secrets behind its inner workings" },
                    { 2, "Steven S. Skiena", 1, false, 30000.0, "The Algorithm Design Manual” is an introduction to creating algorithms on your own from scratch" },
                    { 3, " Robert Sedgewick and Kevin Wayne ", 1, false, 40000.0, "A simple title for a not so simple book, “Algorithms” is incredibly succinct in its naming and belies the full depth of what it covers" },
                    { 4, "E. Oberg", 2, false, 30000.0, "Is it possible to find a book with all the information you need on machinery, metalworking, manufacturing, and even architecture" },
                    { 5, "An Introduction to Mechanical Engineering by J. Wickert and  K. Lewis", 2, false, 4000.0, "A simple title for a not so simple book, “Algorithms” is incredibly succinct in its naming and belies the full depth of what it covers" },
                    { 6, "Mechanics of Fluids", 2, false, 3000.0, "This work should lie on the bedside table of every mechanical engineer who is constantly improving their knowledge" },
                    { 7, "Calculus: Early Transcendentals by James Stewart", 3, false, 10000.0, "It’s written in a very easy and intuitive style and has a ton of exercises." },
                    { 8, "Calculus by Michael Spivak", 3, false, 8000.0, "Spivak presents calculus as “the first real encounter with mathematics” and doesn’t just teach you the concepts of calculus." },
                    { 9, "Calculus for Dummies by Mark Ryan", 3, false, 7000.0, "Do you have a hard time learning maths? This is the book to change that!" },
                    { 10, "Algebra: Structure and Method, Book 1\r\nby Richard G. Brown", 4, false, 15000.0, "The textbook breaks down basic algebraic theories and elementary practical principles at a middle grade and high school level." },
                    { 11, "Algebra: A Complete Introduction: Teach Yourself\r\nby Hugh Neill", 4, false, 10000.0, "Neill’s textbook covers all the essential algebra topics from basic operations to quadratic equations and logarithms. This introduction is designed to help students improve their knowledge with clear explanations of algebra theory as well as examples and test questions" },
                    { 12, "Abstract Algebra\r\nby David Dummit and Richard Foote", 4, false, 2000.0, "Abstract Algebra advances key concepts that underpin algebraic structures. Dummit and Foote begin with the most basic theories and build on existing knowledge with numerous examples to help students best appreciate the mathematical theory and its" },
                    { 13, " Mark J. Price", 5, false, 10000.0, " This comprehensive guide is perfect for beginners, exploring both C# 10 and .NET 6 in detail. Mark J. Price's approach ensures that readers not only learn the syntax but also understand the application of C# in creating modern, high-performing applications. The book includes numerous examples, exercises, and real-world scenarios." },
                    { 14, "Jennifer Greene and Andrew Stellman", 5, false, 12000.0, "Dive into C# and create apps, user interfaces, games, and more using this fun and highly visual introduction to C#, .NET Core, and Visual Studio. With this completely updated guide, which covers C# 8.0 and Visual Studio 2019, beginning programmers like you will build a fully functional game in the opening chapter." },
                    { 15, " Jon Skeet", 5, false, 1000.0, "C# in Depth is known for its thorough exploration of C#. Although it can be somewhat advanced, the book starts with fundamental concepts and gradually moves to more complex topics, making it a valuable resource for beginners aiming to deepen their knowledge. It's revered for its in-depth and meticulous examination of modern C#" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookCategoryId",
                table: "Books",
                column: "BookCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookId",
                table: "Orders",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BookCategories");
        }
    }
}
