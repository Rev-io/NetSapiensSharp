# NetSapiensSharp

An easy-to-use .NET client library for NetSapiens REST API.  

# How to add to your project

Add to your project using Nuget.  

From Visual Studio, click the Tools menu -> NuGet Package Manager -> Package Manager Console.

From the console, run the following command:

`Install-Package NetSapiensSharp`

# Usage

## Overview

All examples below assume you have completed this and have `connector` instantiated

    class Example 
    {
      static string API_BASE_URL = "http://ip.to.netsapiens/ns-api";
      static string CLIENT_ID = "clientId";
      static string CLIENT_SECRET = "ea52004d4d167c65198f211b33a989ae";
      static string USERNAME = "100@domain";
      static string PASSWORD = "123456";
  
      static void Main()
      {
        using (var connector = new Connector(API_BASE_URL, CLIENT_ID, CLIENT_SECRET, USERNAME, PASSWORD)) 
        {
          var domains = Objects.Domain.List(connector, territory: "territory_name");    
        }
      }
    }

## Phone Numbers

### List by Domain & User
    // where connector was created in basic example at beginning of readme
    // where domain_name is the company's domain and 123 is the extension number
    var list = Objects.PhoneNumber.ListByUser(connector, "domain_name", "123");

### Create a Phone Number on a Domain
    var response = Objects.PhoneNumber.CreateForDomain(connector, "domain_name", "2125551234");
    
### Assign a Phone Number to a User
    // where domain_name is the domain, 2125551234 is the DID to assign, and 123 is the extension to assign to
    var response = Objects.PhoneNumber.AssignToUser(connector, "domain_name", "2125551234", "123");

## Domains

### List by Territory
    var list = Objects.Domain.List(connector, territory: "territory_name");

### Create a Domain
    var domain = new Objects.Domain.item() { 
      domain = "domain_name", 
      territory = "territory_name", 
      dial_plan = "dial_plan", 
      description = "example description" 
    };
    var response = Objects.Domain.Create(connector, domain);

### Update a Domain  
    var domain = new Objects.Domain.item() { 
      domain = "domain_name", 
      territory = "territory_name", 
      dial_plan = "dial_plan", 
      description = "new description" 
    };
    var response = Objects.Domain.Update(connector, domain);

### Delete a Domain
    var response = Objects.Domain.Delete(connector, domain: "domain_to_delete");

## Connections

### List Connections
    // examples to be written 
### Create a Connection
    // examples to be written 
### Update a Connection
    // examples to be written 
### Delete a Connection
    // examples to be written 

## Devices

### List Devices
    // examples to be written 
### Create a Device
    // examples to be written 
### Update a Device
    // examples to be written 
### Delete a Device
    // examples to be written 
    
## Users

### List Users
    // examples to be written 
### Create a User
    // examples to be written 
### Update a User
    // examples to be written 
### Delete a User
    // examples to be written 

