using System;
using BCrypt.Net;

public class AuthService{
    private IUserRepository userRepo;

    public AuthService(IUserRepository userRepo){
        this.userRepo=userRepo;
    }

    public bool register(string username,string password){
        if(userRepo.userExists(username))return false;

        string hash=BCrypt.Net.BCrypt.HashPassword(password);
        return userRepo.saveUser(username,hash);
    }

    public bool login(string username,string password){
        var user=userRepo.getUser(username);
        if(user==null)return false;

        if(BCrypt.Net.BCrypt.Verify(password,user.passwordHash))return true;
        return false;
    }
}
