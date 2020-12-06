using System;
using System.Collections.Generic;
using System.Globalization;

namespace Travel_Titan
{
    /// <summary>
    /// Cities to travel to
    /// </summary>
    public enum City
    {
        NONE,
        ATLANTA,
        BOSTON,
        CHARLOTTE,
        CHICAGO,
        DALLAS,
        DENVER,
        DETROIT,
        HONOLULU,
        INDIANAPOLIS,
        LAS_VEGAS,
        LOS_ANGELES,
        MIAMI,
        MINNEAPOLIS,
        MILMAUKEE,
        NASHVILLE,
        NEW_ORLEANS,
        NEW_YORK,
        OKLAHOMA_CITY,
        PHOENIX,
        PITTSBURGH,
        PORTLAND,
        SEATTLE,
        ST_LOUIS,
        WASHINGTON_DC
        
    }

    //
    //********************************************************************
    //  Title: Travel Titan
    //  Application Type: Console
    //  Description: This application will take inputs to find distances
    //               that the user would need to travel
    //  Author: Fewins, Dylon P
    //  Date Created: 11/10/2020
    //  Last Modified: 12/5/2020
    //********************************************************************
    //
    class Program
    {
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayProdeScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// *****************
        /// *  Main Screen  *
        /// *****************
        /// </summary>
        static void DisplayProdeScreen()
        {
            Console.Clear();
            Console.CursorVisible = true;

            (double xComponent, double yComponent) cordinate;
            (double startX, double startY, string stop) cityone;
            (double endX, double endY, string stop) citytwo;
            double distance;
            int userInput;
            int x;
            int stops;
            int cityCount = 1;
            string city;

            List<City> cities = new List<City>();

            DisplayScreenHeader("Planner");

            //
            //Lists options
            //
            Console.WriteLine();
            Console.WriteLine("\tList of Travel Locations");
            Console.WriteLine();
            Console.Write("\t");
            foreach (string cityName in Enum.GetNames(typeof(City)))
            {
                Console.Write($"-{cityName}-");
                if (cityCount % 5 == 0) Console.Write("-\n\t");
                cityCount++;
            }

            //
            //decides starting point
            //
            Console.WriteLine();
            Console.WriteLine("\tHow many stops will you take? [intger]");
            userInput = int.Parse(Console.ReadLine());
            stops = DisplayValidation(userInput);
            Console.WriteLine();
            Console.WriteLine("\tIn Which city will you start your trip?");
            city = Console.ReadLine();
            cityone = CityOptions(city, stops);

            //
            //Data holders
            //
            string[] cityNames = new string[stops + 1];
            double[] distances = new double[stops];

            if (city.ToUpper() != "NONE")
            {

                for (x = 1; x < stops + 1; ++x)
                {

                    cityNames[0] = cityone.stop;

                    Console.WriteLine("\tNow which city will you go to next on your trip?");
                    city = Console.ReadLine();
                    Console.WriteLine();

                    citytwo = CityOptions(city, stops);
                    cityNames[x] = citytwo.stop;

                    cordinate.xComponent = citytwo.endX - cityone.startX;
                    cordinate.yComponent = citytwo.endY - cityone.startY;

                    distance = PythagoreanTheorem(cordinate.xComponent, cordinate.yComponent);
                    distances[x - 1] = distance;

                    Console.WriteLine();
                    Console.WriteLine($"\tThe distance you will have traveled is {distance:n2} miles.");
                    Console.WriteLine();

                    cityone.startX = citytwo.endX;
                    cityone.startY = citytwo.endY;

                }


                DisplayContinuePrompt();

                DisplayTable(stops, cityNames, distances);

            }

            //
            //Used for demo
            //
            //Console.WriteLine("\tNow which city will you end your trip?");
            //city = Console.ReadLine();
            //citytwo = CityOptions(city);

            //cordinate.xComponent = citytwo.endX - cityone.startX;
            //cordinate.yComponent = citytwo.endY - cityone.startY;

            //distance = PythagoreanTheorem(cordinate.xComponent, cordinate.yComponent);


            //Console.WriteLine();
            //Console.WriteLine($"\tThe distance you will have traveled is {distance:n2} miles.");

            DisplayContinuePrompt();


        }

        /// <summary>
        /// Checks for a valid number
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns>A valid answer</returns>
        static int DisplayValidation(int userInput)
        {
            bool validAnswer = false;
            
            while (!validAnswer)
            {
                if (userInput < 0)
                {
                    Console.WriteLine("\tPlease enter a valid number");
                    userInput = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                }

                else
                {
                    validAnswer = true;
                }
            }

            return userInput;
        }


        /// <summary>
        /// *******************************
        /// *  Makes a Table of the trip  *
        /// *******************************
        /// </summary>
        /// <param name="stops"></param>
        /// <param name="cityNames"></param>
        /// <param name="distances"></param>
        static void DisplayTable(int stops, string[] cityNames, double[] distances)
        {
            DisplayScreenHeader("Table");

            double distance;
            double totalDistance=0;
            double average;
            double price;
            string city;
            string cityStart;
            string cityEnd;


            //
            //Table Header
            //
            Console.WriteLine(
                "Start".PadLeft(15) +
                "Stop".PadLeft(15) +
                "Distance".PadLeft(15)
                 );

            Console.WriteLine(
                "------------".PadLeft(15) +
                "------------".PadLeft(15) +
                "------------".PadLeft(15));

            //
            //Table Display
            //
            for(int index =0; index < stops; ++index)
            {
                distance = distances[index];
                cityStart = cityNames[index];
                cityEnd = cityNames[index + 1];
                Console.WriteLine(
                    $"{cityStart}".PadLeft(15) +
                    $"{cityEnd}".PadLeft(15) +
                    $"{distance:n2}".PadLeft(15)
                    );

                totalDistance = totalDistance + distance;
            }

            //
            //Array sum, sort, and average
            //
            Array.Sort(cityNames);
            average = totalDistance / stops;
            price = 50*(stops-1) + (totalDistance*0.11);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"\tTotal Distance: {totalDistance:n2} miles");
            Console.WriteLine($"\tAverage Distance per Trip: {average:n2} miles");
            Console.WriteLine($"\tTotal Price of the trip: {price.ToString("C", CultureInfo.GetCultureInfo("en-US"))}");
            Console.WriteLine("\tList of Cities Visted:");

            for(int index =0 ; index < stops + 1; ++index)
            {
                city = cityNames[index];

                Console.WriteLine($"\t{city}");
                
            }
            
        }

        /// <summary>
        /// Infromation about the City
        /// </summary>
        /// <param name="startCity"></param>
        /// <returns></returns>
        static (double startX, double startY, string stop) CityOptions(string city, int stops)
        {
            (double X, double Y, string stop) cityCord;
            cityCord.X = 0;
            cityCord.Y = 0;
            cityCord.stop = "Nowwhere";
            bool validResponse = false;
            //City city;

            while (!validResponse)
            {

                switch (city.ToUpper())
                {
                    case "NONE":
                        Console.WriteLine();
                        Console.WriteLine("\tI'm sorry you don't want to travel with us");
                        Console.WriteLine();
                        validResponse = true;
                        break;

                    case "ATLANTA":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Big Peach.");
                        Console.WriteLine();
                        cityCord.X = 2321.2083;
                        cityCord.Y = -5825.5113;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "BOSTON":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Athens of America.");
                        Console.WriteLine();
                        cityCord.X = 2923.2264;
                        cityCord.Y = -4899.6624;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "CHARLOTTE":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Queen City.");
                        Console.WriteLine();
                        cityCord.X = 2429.7936;
                        cityCord.Y = 5585.3637;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "CHICAGO":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Windy City.");
                        Console.WriteLine();
                        cityCord.X = 2896.4337;
                        cityCord.Y = -6060.7461;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "DALLAS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Metroplex.");
                        Console.WriteLine();
                        cityCord.X = 2270.0862;
                        cityCord.Y = -6695.7807;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "DENVER":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Mile High city.");
                        Console.WriteLine();
                        cityCord.X = 2750.0709;
                        cityCord.Y = -7222.4853;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "DETROIT":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Motor City.");
                        Console.WriteLine();
                        cityCord.X = 2912.9178;
                        cityCord.Y = -5751.5226;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "HONOLULU":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Big Pineapple.");
                        Console.WriteLine();
                        cityCord.X = 1471.5113;
                        cityCord.Y = -10896.8319;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "INDIANAPOLIS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Circle City.");
                        Console.WriteLine();
                        cityCord.X = 2740.4661;
                        cityCord.Y = -5954.3964;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "LAS VEGAS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Sin City.");
                        Console.WriteLine();
                        cityCord.X = 2489.8857;
                        cityCord.Y = -7945.6260;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "LOS ANGELES":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the City of Angels.");
                        Console.WriteLine();
                        cityCord.X = 2341.9416;
                        cityCord.Y = -8170.1865;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "MIAMI":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Magic City.");
                        Console.WriteLine();
                        cityCord.X = 1779.9171;
                        cityCord.Y = -5539.8099;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "MINNEAPOLIS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Twin Cities.");
                        Console.WriteLine();
                        cityCord.X = 3097.0512;
                        cityCord.Y = -6432.3387;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "MILWAUKEE":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Cream City.");
                        Console.WriteLine();
                        cityCord.X = 2963.3844;
                        cityCord.Y = -6064.8654;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "NASHVILLE":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Music City.");
                        Console.WriteLine();
                        cityCord.X = 2492.7147;
                        cityCord.Y = -5980.7406;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "NEW ORLEANS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Mardi Gras City.");
                        Console.WriteLine();
                        cityCord.X = 2069.3859;
                        cityCord.Y = -6227.8848;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "NEW YORK":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Big Apple.");
                        Console.WriteLine();
                        cityCord.X = 2804.2497;
                        cityCord.Y = -5090.6889;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "OKLAHOMA CITY":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Big Friendly.");
                        Console.WriteLine();
                        cityCord.X = 2804.2497;
                        cityCord.Y = -5090.6889;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "PHOENIX":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Silicon Desert.");
                        Console.WriteLine();
                        cityCord.X = 2307.1737;
                        cityCord.Y = -7728.5382;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "PITTSBURGH":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Steel City.");
                        Console.WriteLine();
                        cityCord.X = 2793.9411;
                        cityCord.Y = -5536.2288;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "PORTLAND":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Rip City.");
                        Console.WriteLine();
                        cityCord.X = 3145.6962;
                        cityCord.Y = -8459.0619;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "SEATTLE":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Jet City.");
                        Console.WriteLine();
                        cityCord.X = 3274.0638;
                        cityCord.Y = -8439.3072;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "ST LOUIS":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Gateway to the West.");
                        Console.WriteLine();
                        cityCord.X = 2673.5223;
                        cityCord.Y = -6235.2126;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    case "WASHINGTON DC":
                        Console.WriteLine();
                        Console.WriteLine("\tYou chose the Capital.");
                        Console.WriteLine();
                        cityCord.X = 2687.7639;
                        cityCord.Y = -5344.4985;
                        cityCord.stop = city;
                        validResponse = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a listed option. If The city has two words the underscore is not included");
                        Console.WriteLine("\t Sorry for the inconvenience.");
                        Console.WriteLine();
                        city = Console.ReadLine();
                        break;

                }
            }

            return cityCord;
        }

        /// <summary>
        /// Pythagorean's Theorem
        /// </summary>
        /// <param name="xComponent"></param>
        /// <param name="yComponent"></param>
        /// <returns>distance from location to location</returns>
        static double PythagoreanTheorem(double xComponent, double yComponent)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow(xComponent, 2) + Math.Pow(yComponent, 2));

            return distance;
        }

        /// <summary>
        /// setup Console Theme
        /// </summary>
        private static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        #region USER INTERFACE

        ///<summary>
        ///********************
        ///*  Welcome Screen  *
        ///********************
        ///</summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tTravel Titan");
            Console.WriteLine();
            Console.WriteLine("\tWelcome User");
            Console.WriteLine();
            Console.WriteLine("\tThis application aims to help you plan a trip");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        ///<summary>
        ///********************
        ///*  Closing Screen  *
        ///********************
        ///</summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tTravel Titan");
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using the Travel Titan");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue propmt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
