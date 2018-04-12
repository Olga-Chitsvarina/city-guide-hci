using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityAttractionsAndEvents
{
    class Place : IComparable
    {
        public string name;
        public int placeType;                           //1 attraction, 2 event, 3 restauant, 4 sports, 5 shopping
        public double starRating;
        public int numOfReviews;
        public double obscurityRating;
        public double price;
        public double posLeft;
        public double posTop;
        public string imagePath;
        public string details;
        public double impObsc;
        public double impPrice;
        public double impStar;
        public Place(string name, int placeType, double starRating, int numOfReviews, double obscurityRating, double price, double posLeft, double posTop)
        {
            this.name = name;
            this.placeType = placeType;
            this.starRating = starRating;
            this.numOfReviews = numOfReviews;
            this.obscurityRating = obscurityRating;
            this.price = price;
            this.posLeft = posLeft;
            this.posTop = posTop;
        }
        public Place(string name, int placeType, double starRating, int numOfReviews, double obscurityRating, double price, double posLeft, double posTop, string imagePath, string details)
        {
            this.name = name;
            this.placeType = placeType;
            this.starRating = starRating;
            this.numOfReviews = numOfReviews;
            this.obscurityRating = obscurityRating;
            this.price = price;
            this.posLeft = posLeft;
            this.posTop = posTop;
            this.imagePath = imagePath;
            this.details = details;
        }

        public int CompareTo(object obj)
        {
            double thisValue = this.starRating / 5 * impStar + this.obscurityRating / 100 * impObsc - this.price / 150 * impPrice;
            Place otherPlace = obj as Place;
            double otherValue = otherPlace.starRating / 5 * impStar + otherPlace.obscurityRating / 100 * impObsc - this.price / 300 * impPrice;
            if (thisValue < otherValue) return 1;
            else if (thisValue > otherValue) return -1;
            return 0;
        }

        public void setPriorities(double obsc, double price, double star)
        {
            this.impObsc = obsc;
            this.impPrice = price;
            this.impStar = star;
        }

        public Place()
        {

        }
    }
}
