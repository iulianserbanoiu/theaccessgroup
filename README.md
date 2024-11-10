# The challenge

How to run:

Command line:
1. Go to HotelAvailability solution folder 
2. Open a CMD window at the path above
3. Run command: dotnet build
4. Run command: dotnet run --hotels hotels.json --bookings bookings.json
5. Run commands specified in the challenge description

Visual Studio:
1. Go to HotelAvailability folder and open solution
2. Set as startup project HotelAvailability
3. Press F5 or debug 
4. Run in the command line the commands specified in the challenge description




The Challenge: 
 
Create a program to manage hotel room availability and reservations. The application should read from files containing hotel data and reservation data, then allow a user to check room availability for a specified hotel, date range, and room type.  
Example command to run the program:  
myapp --hotels hotels.json --bookings bookings.json   
 
Example: hotels.json  
[ { "id": "H1", "name": "Hotel California", "roomTypes": [ {"code": "SGL", “size”: 1 "description": "Single Room", "amenities": ["WiFi", "TV"], "features": ["Non-smoking"]}, {"code": "DBL", "size": 2, "description": "Double Room", "amenities": ["WiFi", "TV", "Minibar"], "features": ["Non-smoking", "Sea View"]} ], "rooms": [ {"roomType": "SGL", "roomId": "101"}, {"roomType": "SGL", "roomId": "102"}, {"roomType": "DBL", "roomId": "201"}, {"roomType": "DBL", "roomId": "202"} ] } ]   
Example: bookings.json  
[ {"hotelId": "H1", "arrival": "20240901", "departure": "20240903", "roomType": "DBL", "roomRate": "Prepaid"}, {"hotelId": "H1", "arrival": "20240902", "departure": "20240905", "roomType": "SGL", "roomRate": "Standard"}]   
 
The program should implement 2 commands described below. 
The program should exit when a blank line is entered.  
 
Availability Command 
 
Example console input: 
Availability(H1, 20240901, SGL) 
Availability(H1, 20240901-20240903, DBL)    
 
Output: the program should give the availability as a number for a room type that date range. Note: hotels sometimes accept overbookings so the value can be negative to indicate this. 
 
RoomTypes Command 
 
Example console input:  
RoomTypes(H1, 20240904, 3)  
RoomTypes(H1, 20240905-20240907, 5)  
 
Output: The program should return a hotel name, and a comma separated list of room type codes needed to allocate number of people specified in the specified time. Minimise the number of rooms to accommodate the required number of people. Avoid partially filling rooms. If a room is partially filled, the room should be marked with a "!”. 
Show an error message if allocation is not possible. 
 
Example output:   
H1: DBL, DBL! 
H1: DBL, DBL, SGL 
 Please approach the challenge as you would a typical work task, keeping in mind principles like testability and extendability. We aren’t looking for advanced frameworks or obscure language features; focus on clean, maintainable code. Most importantly, keep it simple.
 