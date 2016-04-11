function endPointManagerCtor() {
    var self = this;

    self.registrationNewUserUri = '../api/Security/RegisterNewUser';
    self.userNameValidationUri = '../api/Security/UserNameValidation';
    self.emailValidationUri = '../api/Security/EmailValidation';
    self.emailVerificationUri = '../api/Security/VerifyEmailAddress';
    self.changePasswordUri = '../api/Security/ChangePassword';
    self.getStatesUri = '../api/ReferenceData/GetStates';
    self.getCountriesUrl = '../api/ReferenceData/GetCountries';
    self.updateUserProfileUri = '../api/Security/UpdateUserProfile';
    self.retrieveUserProfileUri = '../api/Security/RetrieveUserProfile';
    self.validatePasswordUri = '../api/Security/ValidatePassword';
    self.generateresetPasswordLinkUri = '../api/Security/GeneratePasswordResetLink';
    self.resetPasswordUri = '../api/Security/ResetPassword';
}

angular.module('registrationApp')
  .factory('localServer', function ($http, $location, $window) {
      function localServerConstructor() {
          var self = this;
          self.countries = [];
          var cachedStates = [];
          var endpointsManager = new endPointManagerCtor();
          var backendCommunicationCallbacks = [];

         //*** METHODS (PUBLIC) ***
          self.availableStates = function(country) {
              var result = [];

              if (country) {
                  var countryId = country.id;

                  cachedStates.forEach(function (state) {
                      if (state.countryId == countryId) {
                          result.push(state);
                      }
                  });
              }
              
              return result;
          }

          self.addBackOfficeCommunicationCallback = function(callback) {
              backendCommunicationCallbacks.push(callback);
          }

          self.updateUserProfile = function(edittedUser, successCallback) {
              var uri = endpointsManager.updateUserProfileUri;
              edittedUser.countryId = edittedUser.country.id;

              httpPost(uri,edittedUser, "/saved", successCallback);
          }

          self.changePassword = function (chgPwdInfo, successCallback, errorCallback) {
              chgPwdInfo.userId = self.getCurrentUserId();
             
              httpPost(endpointsManager.changePasswordUri, chgPwdInfo,
                  "/saved",
                  successCallback,
                  errorCallback);
            
          }

          self.navigateTo = function(subResource) {
              //the $location service will make sure any querystring parameters
              //get passed along.
              $location.path(subResource);
          }

          self.registerNewUser = function (user) {
              user.countryId = user.country.id;
              httpPost(endpointsManager.registrationNewUserUri, user, 'confirmEmail');
          }
        
          self.validateEmailAddress = function (newEmailAddress, origEmail, callback) {
              var data = null;
              if (newEmailAddress) {
                  if (!origEmail || origEmail === "") {
                      data = { payload: newEmailAddress };
                  } else if (origEmail != newEmailAddress) {
                      data = { payload: newEmailAddress + ',' + origEmail }
                  };

                  if (data != null) {
                      httpPost(endpointsManager.emailValidationUri, data, null, callback);
                  }
              }
          }

          self.getCurrentUserId = function () {
              var uid = $location.search()["uid"];
              if (!isNaN(uid)) {
                  return uid;
              }

              return 0;
          }

          self.getRedirectURL = function() {
              var redirect = $location.search()["redirect"];
              if (! redirect) {
                  redirect = $location.search()["redirectUrl"];
              }

              return redirect;
          }

          self.validatePassword = function (password, callback) {
              if (password) {
                  var data = { payload: password };
                  httpPost(endpointsManager.validatePasswordUri, data, null, callback);
              }
          }

          self.requestPasswordResetLink = function (resetPasswordInfo, callback) {
              var data = { clientIdRedirect: resetPasswordInfo.clientIdRedirect, usersEmailAddress: resetPasswordInfo.usersEmailAddress };
              httpPost(endpointsManager.generateresetPasswordLinkUri, data, null, callback, null);
          }

          self.resetPassword = function (resetPasswordData) {
              httpPost(endpointsManager.resetPasswordUri, resetPasswordData, null, function (result) { $window.location.href = result.payload }, null);
          }

          
          //This method is down here so 'self' can be defined before calling this method.
          initializeLocalServer();

         //** PRIVATE FUNCTIONS ****
         //Get the list of countries and states from the server
          function initializeLocalServer() {
              //First get the Countries,
              //Then get the states,
              //Once you have all 3, get the user being editted

            httpGet(endpointsManager.getCountriesUrl, null, null, function(countries) {
                self.countries.push.apply(self.countries, countries);

                httpGet(endpointsManager.getStatesUri, null, null, function (states) {
                    cachedStates.push.apply(cachedStates, states);
                    retrieveLogedInUser();
                });
            });
         }
          
         function retrieveLogedInUser(userId) {
             var uri = endpointsManager.retrieveUserProfileUri;

             if (!userId || isNaN(userId) || userId == null) {
                 userId = self.getCurrentUserId();
             }

             if (!isNaN(userId) && userId > 0) {
                 httpGet(uri, userId, null, function (loggedInUser) {
                     //The select dropdown for state needs the stateId to be a string '' not a number
                     loggedInUser.stateId = loggedInUser.stateId.toString();

                     loggedInUser.originalEmail = loggedInUser.email;

                     //before passing the logged in user back up to the controller, set the country object
                     var selectecdCountry = self.countries.filter(function (country) { return country.id == loggedInUser.countryId; });
                     if (selectecdCountry && selectecdCountry.length > 0) {
                         loggedInUser.country = selectecdCountry[0];
                     }

                     self.retrieveLoggedInUserCallback(loggedInUser);
                 });
             }
         }

         function notifyWeAreCommuncatingWithTheBackOffice(url) {
             backendCommunicationCallbacks.forEach(function(backOffice) {
                 backOffice("START", url);
             });
         }

         function notifyWeAreFinishedCommuncatingWithTheBackOffice(url) {
             backendCommunicationCallbacks.forEach(function (backOffice) {
                 backOffice("DONE", url);
             });
         }

         function httpGet(getUrl, data, redirectOnSuccess, callBack, errorCallback) {
              self.lastWebAPIResult = {};
              self.lastWebAPIError = {};

              notifyWeAreCommuncatingWithTheBackOffice(getUrl);

              $http({
                      url: getUrl,
                      method: 'GET',
                      params: {
                          'request.payload': data
                      }
                  })
                  .success(function (getResults) {
                      self.lastWebAPIResult = getResults;

                      if (getResults.isSuccessful)
                          {
                          if (redirectOnSuccess) {
                              $location.path(redirectOnSuccess);
                          }

                          if (callBack) {
                              callBack(getResults.payload);
                          }
                      }
                      else {
                          self.lastWebAPIError = getResults.errorMessage;
                          if (errorCallback) {
                              errorCallback(getResults.errorMessage);
                          }
                          else if (self.errorCallback) {
                              self.errorCallback(getResults.errorMessage);
                          }
                      }

                      notifyWeAreFinishedCommuncatingWithTheBackOffice(getUrl);
                  })
                  .error(function (error) {                     
                      self.lastWebAPIError = error;
                      
                      if (errorCallback) {
                          errorCallback("An error has occured processing your request. Please call IT support to continue.");
                      }
                      else if (self.errorCallback) {
                          self.errorCallback("An error has occured processing your request. Please call IT support to continue.");
                      }

                      notifyWeAreFinishedCommuncatingWithTheBackOffice(getUrl);
                  });
          }

         function httpPost(url, data, redirectOnSuccess, callBack, errorCallBack) {
              self.lastWebAPIResult = {};
              self.lastWebAPIError = {};

              notifyWeAreCommuncatingWithTheBackOffice(url);

              $http.post(url, data)
                  .success(function (postResult) {
                      self.lastWebAPIResult = postResult;
                      
                      if (postResult.isSuccessful) {
                          if (redirectOnSuccess) {
                              $location.path(redirectOnSuccess);
                          }
                         
                          if (callBack) {
                             callBack(postResult);
                          }
                      }
                      else {
                          if (errorCallBack) {
                              errorCallBack(postResult.errorMessage);
                          }
                          else if (self.errorCallback) {
                             self.errorCallback(postResult.errorMessage);
                          }
                      }
                       
                      notifyWeAreFinishedCommuncatingWithTheBackOffice(url);
                  })
                  .error(function(error) {
                      self.lastWebAPIError = error;
                      if (errorCallBack) {
                          errorCallBack("An error has occured processing your request. Please contact IT support to continue.");
                      } else if (self.errorCallback) {
                          if (error.ErrorMessage.length > 0) {
                              self.errorCallBack(error.ErrorMessage);
                          }
                          else {
                              self.errorCallback("An error has occured processing your request. Please contact IT support to continue.");
                          }
                      }

                      notifyWeAreFinishedCommuncatingWithTheBackOffice(url);
                  });
          }
      }
       
      var localServer = new localServerConstructor();

      return localServer;
  });