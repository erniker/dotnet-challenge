// <copyright file="FraudRadar.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>
namespace Refactoring.FraudDetection
{
    using Refactoring.FraudDetection.Entities;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;


    public class FraudRadar
    {
        public IEnumerable<FraudResult> Check(string filePath = "")
        {
            // Read lines from file
            var orders = ReadFraudFile(filePath);

            // Normalize information
            var normalizedOrders = Normalizations(orders);

            var fraudResults = new List<FraudResult>();

            // CHECK FRAUD
            for (int i = 0; i < normalizedOrders.Count; i++)
            {
                var current = orders[i];
                bool isFraudulent = false;

                for (int j = i + 1; j < normalizedOrders.Count; j++)
                {
                    isFraudulent = false;

                    if (current.DealId == normalizedOrders[j].DealId
                        && current.Email == normalizedOrders[j].Email
                        && current.CreditCard != normalizedOrders[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (current.DealId == normalizedOrders[j].DealId
                        && current.State == normalizedOrders[j].State
                        && current.ZipCode == normalizedOrders[j].ZipCode
                        && current.Street == normalizedOrders[j].Street
                        && current.City == normalizedOrders[j].City
                        && current.CreditCard != normalizedOrders[j].CreditCard)
                    {
                        isFraudulent = true;
                    }

                    if (isFraudulent)
                    {
                        fraudResults.Add(new FraudResult { IsFraudulent = true, OrderId = normalizedOrders[j].OrderId });
                    }
                }
            }

            return fraudResults;
        }

        internal List<Order> ReadFraudFile(string filePath)
        {
            // READ FRAUD LINES
            var orders = new List<Order>();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var items = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var order = new Order
                {
                    OrderId = int.Parse(items[0]),
                    DealId = int.Parse(items[1]),
                    Email = items[2].ToLower(),
                    Street = items[3].ToLower(),
                    City = items[4].ToLower(),
                    State = items[5].ToLower(),
                    ZipCode = items[6],
                    CreditCard = items[7]
                };

                orders.Add(order);
            }

            return orders;
        }
        internal List<Order> Normalizations(List<Order> orders)
        {
            // NORMALIZE
            foreach (var order in orders)
            {
                //Normalize email
                var aux = order.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

                var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

                aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

                order.Email = string.Join("@", new string[] { aux[0], aux[1] });

                //Normalize street
                order.Street = order.Street.Replace("st.", "street").Replace("rd.", "road");

                //Normalize state
                order.State = order.State.Replace("il", "illinois").Replace("ca", "california").Replace("ny", "new york");
            }

            return orders;
        }
    }
}