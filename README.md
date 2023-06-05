# Task Manager

## Information and Software Requirements
You are hired to develop a project management system to manage a list of tasks in a project.
There may be some dependencies among these tasks in the project. For example, let's say you
are managing a software development project. You have a list of tasks that need to be
completed, such as design, coding, testing, and deployment. You know that some tasks are
dependent on others, such as testing cannot start until coding is completed.

The information about the tasks and the dependencies among the tasks are stored in a text file.
The information about a task includes a task ID, which is a string, and the time needed to
complete the task, which is a positive integer, and a list of tasks that the task depends on. The
information about one task is stored in a separated line in the text file. Here is an example,
demonstrating the organisation of the text file:

T1, 100 <br>
T2, 30, T1 <br>
T3, 50, T2, T5 <br>
T4, 90, T1, T7 <br>
T5, 70, T2, T4 <br>
T6, 55, T5 <br>
T7, 50 <br>

The above indicates that the time needed to complete task T1 is 100 and T1 does not depend
on any other tasks in the project, the time needed to complete task T2 is 30 and task T2 cannot
commence until task T1 has been completed, the time needed to complete task T3 is 50 and
task T3 cannot commence until both tasks T2 and T5 have been completed, and so on.
The project management system should have a Microsoft Console Application with a
command line menu allowing the user to do the following:

• Ask the user to enter the name of a text file in which the information about the tasks in
a project and the dependencies among the tasks are stored and read the information
from the text file into the system.

• Add a new task with time needed to complete the task and other tasks that the task
depends on into the project.

• Remove a task from the project.

• Change the time needed to be completed.

• Save the (updated) information about the tasks and dependencies back to the opened
input text file.

• Find a sequence of the tasks that does not violate any task dependency and save the
sequence to a text file, namely, Sequence.txt. For example, for the above sample
project, the content in Sequence.txt should look like:

T1, T2, T7, T4, T5, T3, T6

• Find the earliest possible commencement time for each of the tasks in the project. and
save the solution into a text file, namely, EarliestTimes.txt. For example, for the above
sample project, the content in EarliestTimes.txt should look like:

T1, 0 <br>
T2, 100 <br>
T3, 260 <br>
T4, 100 <br>
T5, 190 <br>
T6, 260 <br>
T7, 0 <br>


![task-manager](https://github.com/ben04rogers/task-manager/assets/47819009/3eb6e691-09b7-4cb3-b2be-b8a22bee877e)
