SampleCode
==========

Imagine Design Layout
-------------

Controllers: are the Rest entry point and return ViewModel inside HttpResponse

ViewModels:  call onto Models for Data and then translate/cast into ViewModel

Models:      call direcly onto webservices and returns webservice object

Filters:     are used to catch exceptions, apply auth and log message.
