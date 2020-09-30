# LiftEngine
## Problem:
You are in charge of writing software for an elevator (lift) company.

Your task is to write a program to control the travel of a the lift for a 10 storey building.
A passenger can summon the lift to go up or down from any floor, once in the lift they can choose the floor they’d like to travel to.
Your program needs to plan the optimal set of instructions for the lift to travel, stop, and open its doors.

Some test cases:
* Passenger summons lift on the ground floor. Once in chooses to go to level 5.
* Passenger summons lift on level 6 to go down. Passenger on level 4 summons the lift to go down. They both choose L1.
* Passenger 1 summons lift to go up from L2. Passenger 2 summons lift to go down from L4. Passenger 1 chooses to go to L6. Passenger 2 chooses to go to Ground Floor
* Passenger 1 summons lift to go up from Ground. They choose L5. Passenger 2 summons lift to go down from L4. Passenger 3 summons lift to go down from L10. Passengers 2 and 3 choose to travel to Ground.

You can implement this in any language you like and submit a working solution including tests and a readme explaining how to run it. Submissions on GitHub or BitBucket preferred (public repo is fine).

## Solution:
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
