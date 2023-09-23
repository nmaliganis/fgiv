const defaultUser = {
  email: '',
  avatarUrl: ''
};

export default {
  _user: defaultUser,
  loggedIn() {
    return !!this._user;
  },
};
