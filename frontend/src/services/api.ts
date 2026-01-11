import axios, { AxiosResponse } from 'axios';
import {
  LoginCredentials,
  RegisterData,
  AuthResponse,
  ChangePasswordData,
  ResetPasswordData,
  ConfirmResetPasswordData,
  User,
  UpdateUserData,
  LoginHistoryEntry
} from '../types';

const API_BASE_URL = 'https://localhost:7000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle auth errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: (credentials: LoginCredentials): Promise<AxiosResponse<AuthResponse>> => 
    api.post('/auth/login', credentials),
  register: (userData: RegisterData): Promise<AxiosResponse<AuthResponse>> => 
    api.post('/auth/register', userData),
  createUser: (userData: RegisterData): Promise<AxiosResponse<{ message: string; user: User }>> => 
    api.post('/auth/create-user', userData),
  getRoles: (): Promise<AxiosResponse<Array<{ value: number; name: string; description: string }>>> => 
    api.get('/auth/roles'),
  logout: (): Promise<AxiosResponse<{ message: string }>> => 
    api.post('/auth/logout'),
  changePassword: (passwordData: ChangePasswordData): Promise<AxiosResponse<{ message: string }>> => 
    api.post('/auth/change-password', passwordData),
  resetPassword: (data: ResetPasswordData): Promise<AxiosResponse<{ message: string }>> => 
    api.post('/auth/reset-password', data),
  confirmResetPassword: (data: ConfirmResetPasswordData): Promise<AxiosResponse<{ message: string }>> => 
    api.post('/auth/confirm-reset-password', data),
  getCurrentUser: (): Promise<AxiosResponse<User>> => 
    api.get('/auth/me'),
};

// Users API
export const usersAPI = {
  getAllUsers: (): Promise<AxiosResponse<User[]>> => 
    api.get('/users'),
  getUserById: (id: number): Promise<AxiosResponse<User>> => 
    api.get(`/users/${id}`),
  createUser: (userData: RegisterData): Promise<AxiosResponse<User>> => 
    api.post('/users', userData),
  updateUser: (id: number, userData: UpdateUserData): Promise<AxiosResponse<User>> => 
    api.put(`/users/${id}`, userData),
  deleteUser: (id: number): Promise<AxiosResponse<{ message: string }>> => 
    api.delete(`/users/${id}`),
  toggleUserStatus: (id: number): Promise<AxiosResponse<{ message: string }>> => 
    api.patch(`/users/${id}/toggle-status`),
  getUserLoginHistory: (id: number): Promise<AxiosResponse<LoginHistoryEntry[]>> => 
    api.get(`/users/${id}/login-history`),
};

export default api;