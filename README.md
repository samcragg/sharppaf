# Project description

This library provides utilities to format and extract the raw data from the Royal Mail's Postcode Address File (PAF®). This library does NOT provide any data from the PAF nor provide any ability to search for data (i.e. how to find an address).

The main aim of the library is to allow the extraction of the raw data from the PAF® file to store in a relational database and to combine the address components into a printable form (a simple example is the building number and building name are stored in two fields but these should be on the same line when printing/displaying the address) with the option of converting the UPPERCASE text to TitleCase using additional logic to match the CSV versions of the PAF® data (e.g. MCDONALD converts to McDonald). The library also includes validation/formatting of Postcodes, which can be used to verify that the user entered a Postcode in the correct format and/or to normalize the user entered Postcode.

## PAF® description

The PAF® is a database containing all known addresses and Postcodes in the UK, holding over 29 million addresses and 1.8 million Postcodes. It is supplied and maintained by the Royal Mail and the raw data is available to external organisations. It is this raw data that SharpPaf can parse and format into printable addresses. For more information about the raw data and its licencing visit [http://www.poweredbypaf.com](http://www.poweredbypaf.com).

## Importing data

The SharpPaf.Data.dll includes the portable SharpPaf classes and additional classes that can be used to parse the raw PAF® files. These can then be saved in a database, such as a SQL database or a simple key/value NoSQL database, using the repository pattern to allow easy integration with other code. Optionally the case of this data can be changed when saving to the repository, slowing importing time but allows quicker formatting if the outputted text is wanted in TitleCase.