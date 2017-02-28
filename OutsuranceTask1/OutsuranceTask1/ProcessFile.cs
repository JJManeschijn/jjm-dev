using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OutsuranceTask1
{
    public class ProcessFile
    {
        public List<Customer> CustomerSet = null;

        #region Public Members

        public string ValidateCSV(string fileName)
        {
            string result = string.Empty;
            try
            {
                //is a filename defined?
                if (!string.IsNullOrEmpty(fileName))
                {
                    //does the csv file exist?
                    if (!File.Exists(fileName))
                    {
                        //no - does the path exist?
                        string path = Path.GetDirectoryName(fileName);
                        if (!Directory.Exists(path))
                        {
                            result = string.Format(Resources.Resources.InvalidPath, path);
                        }
                        else
                        {
                            string filename = Path.GetFileName(fileName);
                            result = string.Format(Resources.Resources.InvalidFile, filename);
                        }
                    }
                }
                else
                {
                    result = Resources.Resources.SelectFile;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string ValidateCSVHeader(string fileName)
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ValidateCSV(fileName))) //in case user changed the filename to something invalid...
                {
                    //open file and read contents into customer object
                    using (StreamReader streamReader = new StreamReader(fileName))
                    {
                        //read until we get data
                        while (!streamReader.EndOfStream)
                        {
                            var customerLine = streamReader.ReadLine();
                            if (!string.IsNullOrEmpty(customerLine))
                            {
                                if ((customerLine.ToLower().Contains("name")) || (customerLine.ToLower().Contains("address")))
                                {
                                    //assume this is a header
                                    result = "True";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string ProcessCSV(string fileName, bool hasHeader)
        {
            string result = string.Empty;
            int readCount = 0;
            int processCount = 0;
            //open file and read contents into customer object
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                if (hasHeader)
                {
                    streamReader.ReadLine();
                }
                while(!streamReader.EndOfStream)
                {
                    readCount++;
                    var customerLine = ProcessCustomerLine(streamReader.ReadLine());
                }
            }
            if ((CustomerSet != null) && (CustomerSet.Any()))
            {
                processCount = CustomerSet.Count;
            }
            //result = string.Format("{0} lines read, {1} lines processed.", readCount, processCount);
            return result;
        }

        public string ExportNames(string fileName)
        {
            string result = string.Empty;
            try
            {
                //dump ALL names into a single list
                List<string> allNames = new List<string>();
                foreach (Customer customer in CustomerSet)
                {
                    foreach (CustomerName name in customer.CustomerNames)
                    {
                        allNames.Add(name.Name);
                    }
                }
                //group names, then get count and sort descending
                var nameResultSet = allNames.GroupBy(i => i).Select(grp => new { name = grp.Key, count = grp.Count(), }).ToList();
                var sortedNameResultSet = nameResultSet.OrderByDescending(n => n.count).ThenBy(n => n.name);
                //write text file
                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    foreach (var item in sortedNameResultSet)
                    {
                        var line = string.Format("{0},{1}", item.name, item.count);
                        streamWriter.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string ExportAddresses(string fileName)
        {
            string result = string.Empty;
            try
            {
                //sort Adresses by street name, then number (in case the same street)
                var sortedAddressSet = CustomerSet.OrderBy(c => c.CustomerAddress.Name).ThenBy(c => c.CustomerAddress.Number);
                //write text file
                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    foreach (var item in sortedAddressSet)
                    {
                        var line = string.Format("{0} {1}", item.CustomerAddress.Number, item.CustomerAddress.Name);
                        streamWriter.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        #endregion

        #region Private Members

        Customer ProcessCustomerLine(string dataline)
        {
            Customer result = null;
            string[] dataLineSet = dataline.Split(',');
            if (dataLineSet.Length == 2)
            {
                Customer customer = new Customer() { };
                //split name string into separate names
                string[] names = dataLineSet[0].Split(' ');
                foreach(string name in names)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        CustomerName customerName = new CustomerName()
                        {
                            Name = name,
                        };
                        if (customer.CustomerNames == null)
                        {
                            customer.CustomerNames = new List<CustomerName>();
                        }
                        customer.CustomerNames.Add(customerName);
                    }
                }

                CustomerAddress customerAddress = new CustomerAddress() { };
                //split address number and street name
                List<string> streetNameSet = new List<string>();  
                names = dataLineSet[1].Split(' ');
                foreach (string name in names)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        //if name is numeric, it must be the number
                        if (IsNumeric(name))
                        {
                            customerAddress.Number = name;
                        }
                        else
                        {
                            streetNameSet.Add(name);
                        }
                    }
                }
                //get the number if we don't have one
                if (string.IsNullOrEmpty(customerAddress.Number))
                {
                    for (int i = 0; i < streetNameSet.Count; i++)
                    {
                        //if number contains alpha character, it is almost always a single character preceded by the number...
                        if (IsNumeric(streetNameSet[i].Substring(0, streetNameSet[i].Length - 1)))
                        {
                            customerAddress.Number = streetNameSet[i];
                            streetNameSet[i] = string.Empty;
                        }
                    }
                }

                string streetName = string.Empty;
                //build the street name without number
                foreach (string item in streetNameSet)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        streetName += string.Format("{0} ", item);
                    }
                }
                //finally last remove space from street name
                streetName = streetName.Substring(0, streetName.Length - 1);
                customerAddress.Name = streetName;
                
                //add address to customer object
                customer.CustomerAddress = customerAddress;
                //add customer to list
                if (CustomerSet == null)
                {
                    CustomerSet = new List<Customer>();
                }
                CustomerSet.Add(customer);
            }
            return result;
        }

        bool IsNumeric(string value)
        {
            int n;
            bool isNumeric = int.TryParse(value, out n);
            return isNumeric;
        }
        
        #endregion
    }
}
