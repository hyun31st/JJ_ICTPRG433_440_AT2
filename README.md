# JJ_ICTPRG433_440_AT2
Semester 2 Tuesday ICTPRG433_440 AT2 C# Project

# Recruitment System Class Structure

This document provides a structural overview of the core components used in the recruitment management system. The system is designed to manage contractors and jobs, allowing operations such as creation, assignment, tracking, filtering, and reporting of job activities and contractor availability.

It is implemented in C# and follows a simplified object-oriented design, with the `RecruitmentSystem` class serving as the main service layer responsible for coordinating interactions between `Contractor` and `Job` entities.

---

# Class Diagram: Recruitment System

This documentation outlines the structure and relationships between the core classes in the recruitment system.
## Class Structure Overview

### RecruitmentSystem

| Member / Method                         |
|-----------------------------------------|
| `- _contractors`                        |
| `- filteredContractors`                 |
| `- _jobs`                               |
| `- filteredJobs`                        |
| `- outOfRangeFilteredJobs`              |
| `+ AddContractor()`                     |
| `+ RemoveContractor()`                  |
| `+ GetContractors()`                    |
| `+ AddJob()`                            |
| `+ RemoveJob()`                         |
| `+ GetJobs()`                           |
| `+ SortContractorsByFirstName()`        |
| `+ SortContractorsByLastName()`         |
| `+ SortContractorsByHourlyWage()`       |
| `+ ShowAvailableContractorsOnly()`      |
| `+ ShowAllContractors()`                |
| `+ SortJobsByTitle()`                   |
| `+ SortJobsByCost()`                    |
| `+ ShowAvailableJobsOnly()`             |
| `+ ShowInProgressJobs()`                |
| `+ ShowCompletedJobs()`                 |
| `+ JobsFilterbyCostRange()`             |
| `+ ShowAllJobs()`                       |
| `+ AssignContractorToAJob()`            |
| `+ CompleteJob()`                       |
| `+ JobCreateReport()`                   |
| `+ IsContractorAssignedToJob()`         |
| `+ GetContractorByID()`                 |
| `+ AddContractorExamples()`             |
| `+ AddJobExamples()`                    |

---

### Contractor

| Property      |
|---------------|
| `Id`          |
| `FirstName`   |
| `LastName`    |
| `StartDate`   |
| `HourlyWage`  |

---

### Job

| Property             |
|----------------------|
| `Id`                 |
| `Title`              |
| `CreateDate`         |
| `Cost`               |
| `CompleteDate`       |
| `ContractorAssigned` |
| `Remark`             |
| `ContractorId`       |
| `ContractorHours`    |


## Notes

- **RecruitmentSystem**: Central controller managing both `Contractor` and `Job` entities.
- **Contractor**: Represents a worker with basic details like name, availability (StartDate), and pay rate.
- **Job**: Represents a task with lifecycle fields and references to assigned contractors.

### Relationships

- `RecruitmentSystem` holds collections of both `Contractor` and `Job` objects.
- Each `Job` may optionally reference a `Contractor` via `ContractorId`.