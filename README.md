# DADSTORM

Repository for the Distributed Application Development (DAD) course - Instituto Superior Tecnico, 2016/2017

## Abstract

The DAD project, named  *DADSTORM*, aims at implementing a simplified (and therefore far from complete) implementation of a fault-tolerant real-time distributed stream processing system.
Checkout the [project description](DAD-project-description.pdf) for more info.


## Running the project

To run DADSTORM:
- build the solution (`DADStorm.sln`) using MS Visual Studio
- launch one PCS executable per machine where an Operator is to be run
- run the PuppetMaster executable
- load and run the example configuration file (located in the `InputFiles` folder - dadstorm-mini.config is a good start) using the PuppetMaster GUI
