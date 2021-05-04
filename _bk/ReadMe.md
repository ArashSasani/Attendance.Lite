**LogToEntrance project**:
This project is responsible for converting raw data(logs) coming from the devices or upload file which contains
personnel logs to readable entrance records for the whole system.

**DismissalLogToEntrance, DutyLogToEntrance**:
The process here is almost as same as the **LogToEntrance** project.
These projects are responsible for converting dismissal or duty logs to readable entrance records.
These entrances define when the personnel had dismissals(off days or hours) and duties.

**EntranceAdjustment**:
This project is responsible for **closing and opening entrance records for personnel** according to their assigned shifts.
This project contains *schedulers* that can automatically up and run.

All these project work with the *main (backend) database* which has been created by the Ubercontext schema.
All these projects should be used as **windows service on the application/database server**.

You can use [nssm](https://nssm.cc/) for making services out of console apps