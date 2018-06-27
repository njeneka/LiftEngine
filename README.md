# LiftEngine
The solution was developed using Visual Studio .Net 2015.  Technologies: mvc, webapi, reactjs, MS Test.

The application has been deployed to http://liftengine.azurewebsites.net.

## How to Operate the Lift
* Refreshing the page will refresh the lift.
* The current level is shown with a white background (doors open).  All other floors have a black background.
* Select the up and down arrows on each level to summons the lift and select levels to disembark from inside the lift.
* Press Travel to Next Stop when you want to travel.  
* All requests entered prior to travel are serviced independently.  eg. if you summons the lift and select disembark before travel they 
are two separate request (that could have been made by two different passengers) so the lift may stop at the disembark level before the 
summons level.  If you want the requests to be treated as the same passenger you will need to summons the lift, travel and then select 
your disembark level.
* A history of where the lift has stopped is shown below Travel to Next Stop.  To clear the history, and all requests, refresh the page.

### Lift Travel Algorithm
* The initial lift direction is set by the first request.
* The lift will travel in the same direction while it can service requests (summons or disembark) ahead of it in the same direction.
* When there are no further requests ahead the lift will turn around and go to the closest disembark or summons in the reverse direction,
to service the other direction.
* If there are no further requests in the other direction the lift will travel to the furtherest stop requesting the same direction it
is currently moving in and service the same direction again.

#### Assumptions
* The code test requests a 10 storey building.  Suggested test case 4 refers to Level 10 so assuming that a 10 storey building has 11 floors, 
Ground plus 10 levels.
* There is no need to track passengers as per standard lift operation.
* The doors will open automatically when the lift stops and close when travel commences.
