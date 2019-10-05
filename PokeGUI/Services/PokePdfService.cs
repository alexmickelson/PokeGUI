﻿using System;
using System.Collections.Generic;
using System.Text;
using IronPdf;
using PokeGUI.Models;

namespace PokeGUI.Services
{
    public class PokePdfService 
    {
        public bool WritePdf(IEnumerable<Pokemon> pokemonCollection)
        {
            var document = new HtmlToPdf();
            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append(topOfPdf);

            foreach(var pokemon in pokemonCollection)
            {
                htmlBuilder.Append($@"
                    <tr>
                        <td class='no'>{pokemon.PokeId}</td>
                        <td class='desc'>{pokemon.Name}</td>
                        <td class='unit'>{pokemon.Type1}</td>
                        <td class='qty'>{pokemon.Type2}</td>
                        <td class='white'><img src=''></img></td>
                    </tr>");
            }

            htmlBuilder.Append(bottomOfPdf);
            var pdf = document.RenderHtmlAsPdf(htmlBuilder.ToString());
            pdf.Print();
            return true;
        }

        private string topOfPdf = @"<!DOCTYPE html>
< html lang='en'>
  <head>
    <meta charset = 'utf-8' >
    < title > Example 2</title>
    <style>@font-face {
        font-family: SourceSansPro;
        src: url(SourceSansPro-Regular.ttf);
    }
      .clearfix:after {
        content: '';
        display: table;
        clear: both;
      }
a {
        color: #0087C3;
        text-decoration: none;
      }
      body {
        position: relative;
        width: 21cm;  
        height: 29.7cm; 
        margin: 0 auto; 
        color: #555555;
        background: #FFFFFF; 
        font-family: Arial, sans-serif; 
        font-size: 14px; 
        font-family: SourceSansPro;
      }
      header {
        padding: 10px 0;
        margin-bottom: 20px;
        border-bottom: 1px solid #AAAAAA;
      }
      
      #logo {
        float: left;
        margin-top: 8px;
      }
      #logo img {
        height: 70px;
      }
      #company {
        float: right;
        text-align: right;
      }
      #details {
        margin-bottom: 50px;
      }
      #client {
        padding-left: 6px;
        border-left: 6px solid #0087C3;
        float: left;
      }
      #client .to {
        color: #777777;
      }
      h2.name {
        font-size: 1.4em;
        font-weight: normal;
        margin: 0;
      }
      #invoice {
        float: right;
        text-align: right;
      }
      #invoice h1 {
        color: #0087C3;
        font-size: 2.4em;
        line-height: 1em;
        font-weight: normal;
        margin: 0  0 10px 0;
      }
      #invoice .date {
        font-size: 1.1em;
        color: #777777;
      }
      table {
        width: 100%;
        border-collapse: collapse;
        border-spacing: 0;
        margin-bottom: 20px;
      }
      table th,
      table td {
        padding: 20px;
        background: #EEEEEE;
        text-align: center;
        border-bottom: 1px solid #FFFFFF;
      }
      table th
{
    white-space: nowrap;
    font-weight: normal;
}
table td
{
    text-align: right;
}
table td h3{
        color: #57B223;
        font-size: 1.2em;
        font-weight: normal;
        margin: 0 0 0.2em 0;
      }
      table.no {
        color: #FFFFFF;
        font-size: 1.6em;
        background: #57B223;
      }
      table.desc {
        text-align: left;
      }
      table.unit {
        background: #DDDDDD;
      }
      table.qty {
      }
      table.total {
        background: #57B223;
        color: #FFFFFF;
      }
      table td.unit,
      table td.qty,
      table td.total {
    font - size: 1.2em;
}
table tbody tr:last-child td
{
    border: none;
}
table tfoot td {
        padding: 10px 20px;
        background: #FFFFFF;
        border-bottom: none;
        font-size: 1.2em;
        white-space: nowrap; 
        border-top: 1px solid #AAAAAA; 
      }
      table tfoot tr:first-child td
{
    border-top: none;
}
table tfoot tr:last-child td
{
    color: #57B223;
        font-size: 1.4em;
    border-top: 1px solid #57B223; 
      }
table tfoot tr td:first-child {
        border: none;
      }
      #thanks{
        font-size: 2em;
        margin-bottom: 50px;
      }
      #notices{
        padding-left: 6px;
        border-left: 6px solid #0087C3;  
      }
      #notices .notice {
        font-size: 1.2em;
      }
      footer {
        color: #777777;
        width: 100%;
        height: 30px;
        position: absolute;
        bottom: 0;
        border-top: 1px solid #AAAAAA;
        padding: 8px 0;
        text-align: center;
      }      
      .white{
        background: white
      }
    </style>
  </head>
  <body>
    <header class='clearfix'>
    </header>
    <main>
      <div id = 'details' class='clearfix'>
        <div id = 'client' >
                < img style='height: 7em;' src='https://proxy.duckduckgo.com/iu/?u=https%3A%2F%2Ftse4.mm.bing.net%2Fth%3Fid%3DOIP.JOLMiUAW3JyI6ZhX0vXWBgHaHa%26pid%3DApi&f=1'>
          </div>
        <div id = 'invoice' >
          < h1 > PokeList! </ h1 >
          < div class='date'>Due Date: 10/5/2019</div>
        </div>
      </div>
      <table border = '0' cellspacing='0' cellpadding='0'>
        <thead>
          <tr>
            <th class='no'>#</th>
            <th class='desc'>Name</th>
            <th class='unit'>Type1</th>
            <th class='qty'>Type2</th>
            <th class='white'>IMG</th>
          </tr>
        </thead>
        <tbody>";
        private string bottomOfPdf = @"</tbody></table></body</html>";

    }
}