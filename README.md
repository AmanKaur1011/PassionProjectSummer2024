# PassionProjectSummer2024

## Purpose of this project:
I built an Amazon Workplace Management system. In this system, any employee of Amazon can see the list of other employees working among different 
departments, as well as the number of departments in Amazon with the department information and the position or the roles in the company with the 
details about that role. 
The administrator would have the full control over the system to manage it efficiently. Administrator can add new employees to the system, update the
current employee information and can also delete the previous employees. Similarly, the Administrator can also add new departments or a new Position(role), update the current 
department information, or update the current Position details like updating the wage associated with the position, and can also delete the old departments and positions.

### Basic Features of the system:
- Create, read, update, and delete functionality for the employees.
- Create, read, update, and delete functionality for the departments.
- Create, read, update, and delete functionality for the Positions.

## ERD Diagram 
![ERD with colored entities (UML notation) (3)](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/bb2f3a18-4919-4cf1-a7b3-8e0283d7fe9b)

## Challenges Faced:
Incorporating the  Update interface in the Department and position where employees can directly be added to that department or position  was challenging 
as I have a One-to-many relationship between employee and department entities as well as in employee and position entities. So, keeping track of the previous  department ID  and position ID was a bit hard. Eventually, It was successfully incorporated into this passion project.

## Extra Feature Incorporated:
- Authentication is added as the extra feature to this passion project to give the critical functionalities access(like create, update, and delete ) only to authenticated 
  users.
- Using Images, styling, and responsive behavior 

## Features to explore in the Future:
- Search Functionality to search employees by adding a particular name, hire date, position, or department name in the search bar
- Adding the cross-training portal as a separate entity that can keep track of the employees who are cross-trained in other departments. So, when the employee 
  information is shown it also shows what other departments, they are cross -trained.

## Interface Snippets
### Interface for the Home Page with a Carousel
![Screenshot 2024-06-21 033240 (1)](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/f517eedb-b04e-4459-bffd-1fd771ec4196)

![Screenshot 2024-06-21 033248](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/60e99f6b-abff-4865-b31b-a2084f4dfa50)


### Interface showing List of employees
![Screenshot 2024-06-21 033303](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/aad89fd6-e323-423f-ad18-38caf78e0ee9)
<hr>

### Interface Showing the Employee Details
![Screenshot 2024-06-21 033336](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/8df56326-d323-472a-88ba-50c8cfd33b84)
<hr/>


### Interface to update and delete an employee 
![Screenshot 2024-06-21 033404](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/236a08ae-ad0b-4bac-92c3-0f8c64e4774b)

![Screenshot 2024-06-21 033513](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/6dbfc261-a900-45c2-93c0-2e34a431619c)
<hr/>

### Interface to add an employee to the system
![Screenshot 2024-06-21 033322](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/a580ce7f-cf12-4640-b878-83b6d30017b4)

<hr>

### Interface showing the list of departments in the system
![Screenshot 2024-06-21 033521](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/8b0cfac1-bf9e-4b96-8b27-572b6c1dcde5)
<hr>

### Interface showing Department Details with the number of employees in that department
![Screenshot 2024-06-21 033545](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/5a56ebe8-b464-4069-9ca9-5f6418a16266)

<hr>

### Interface to Update a department where employees can be added and deleted to the department
![Screenshot 2024-06-21 033553](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/541eb7fd-6583-4884-afce-d6cf9b96ec18)

<hr>

### Interface  showing the details of a position Entity
![Screenshot 2024-06-21 033610](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/2a2a2134-0481-4485-9892-a558e95c6b4d)
<hr/>


### Similar interface for Position Entity's Update functionality

![Screenshot 2024-06-21 033620](https://github.com/AmanKaur1011/PassionProjectSummer2024/assets/156178926/81878a1f-d3d9-42b5-9019-92f57c99f2c7)



