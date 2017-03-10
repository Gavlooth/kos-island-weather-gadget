using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Windows;
using System.ComponentModel;

namespace WeatherGadget
{

    public class Gadget
    {
        static string Url = "http://penteli.meteo.gr/stations/kos/";                        
        public string celsius;
      
        public Dictionary<string, string> DataScrap()
            {
                Dictionary<string, string> nodeDictionary = new Dictionary<string, string>();
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(Gadget.Url);          
           foreach  (  var node in htmlDoc.DocumentNode.Descendants("font").Skip(15)) //Node number 16 contains the temperature
           {              
               nodeDictionary.Add("Temperature", (node.InnerText.Substring(0, node.InnerText.Length-2)));
               break;
           }        
           
           foreach  (  var node in htmlDoc.DocumentNode.Descendants("font").Skip(20)) //Node number 16 contains the temperature
           {              
               nodeDictionary.Add("Humidity",node.InnerText.Substring(0, node.InnerText.Length-2)+@" %");
               break;
           }
                                  
           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(28)) //Node number 16 contains the temperature
           {
               string tmp1, tmp2, tmp3;
               tmp1 = tmp2 = tmp3 = node.InnerText;        
       
               tmp2 = tmp2.Remove(tmp2.IndexOf(" \r"));
               tmp2 = tmp2.Substring(tmp2.IndexOf("at ")+3);   
          
               tmp1 = tmp1.Remove(tmp1.IndexOf("&")) + " Km/h";

               tmp3 = tmp3.Remove(tmp3.IndexOf(")") - 1);
               tmp3 = tmp3.Substring(tmp3.IndexOf("(") + 1);         

               nodeDictionary.Add("Wind", tmp1+" at "+tmp2 );              
               nodeDictionary.Add("WindAngle", tmp3);              
               break;
           }

           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(34)) //Node number 16 contains the temperature
           {
               string tmp = node.InnerText;               
               tmp = tmp.Replace("\r\n\t", ", ");             
               nodeDictionary.Add("Barometer", tmp);                         
               break;
           }

       
           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(38)) //Node number 16 contains the temperature
           {
               string tmp1=node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("&"));
               nodeDictionary.Add("Today's Rain", tmp1+" mm");
               break;
           }
           
           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(44)) //Node number 16 contains the temperature
           {
              string tmp1;
              tmp1  = node.InnerText;
              tmp1 = tmp1.Remove(tmp1.IndexOf("&"));          
              nodeDictionary.Add("Rain_rate", tmp1 + " mm/h");
              break;
           }           
           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(48)) //Node number 16 contains the temperature
           {       
               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("&"));                
               nodeDictionary.Add("Storm_total", tmp1 + " mm");
        
               break;
           }
        
           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(53)) //Node number 16 contains the temperature
           {
               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("&"));                             
               nodeDictionary.Add("Monthly_rain", tmp1 + " mm");
               break;
           }

           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(62)) //Node number 16 contains the temperature
           {
               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf(" "));
               nodeDictionary.Add("Wind_chill", tmp1 + " °C");             
               break;
           }

           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(70)) //Node number 16 contains the temperature
           {
               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("C")-1);
               nodeDictionary.Add("Heat_index", tmp1 + " °C");               
               break;
             }

           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(79)) //Node number 16 contains the temperature
           {          

               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("&"));
                              nodeDictionary.Add("Sunrise", tmp1 );

             
               break;
           }

           foreach (var node in htmlDoc.DocumentNode.Descendants("font").Skip(82)) //Node number 16 contains the temperature
           {
               string tmp1;
               tmp1 = node.InnerText;
               tmp1 = tmp1.Remove(tmp1.IndexOf("&"));
               nodeDictionary.Add("Sunset", tmp1);
               break;
           }
          
           string temp1;
           string temp2;
           nodeDictionary.TryGetValue("Sunrise", out temp1);
           nodeDictionary.TryGetValue("Sunset", out temp2);
           string time = (TimeSpan.FromMinutes(Convert.ToDateTime("01/01/2015 " + temp2).Subtract(Convert.ToDateTime("01/01/2015 " + temp1)).TotalMinutes).ToString().Remove(5));
           nodeDictionary.Add("Day_length", time);
           return nodeDictionary;
        
           }
    }

}