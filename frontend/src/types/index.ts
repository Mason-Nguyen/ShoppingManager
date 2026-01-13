export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: number; // Backend sends role as integer
  isActive: boolean;
  createdAt: string;
  lastLoginAt?: string;
}

export interface Product {
  id: string;
  code: string;
  name: string;
  unit: string;
  refPrice: number;
  image?: string;
  description?: string;
  createdAt: string;
  updatedAt?: string;
}

export enum UserRole {
  User = 0,
  Admin = 1,
  Requester = 2,
  Approver = 3,
  Receiver = 4,
  Purchase = 5
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  isActive?: boolean;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface ChangePasswordData {
  currentPassword: string;
  newPassword: string;
}

export interface ResetPasswordData {
  email: string;
}

export interface ConfirmResetPasswordData {
  token: string;
  newPassword: string;
}

export interface AdminUpdatePasswordData {
  userId: number;
  newPassword: string;
}

export interface CreateProductData {
  code: string;
  name: string;
  unit: string;
  refPrice: number;
  image?: string;
  description?: string;
}

export interface UpdateProductData {
  code: string;
  name: string;
  unit: string;
  refPrice: number;
  image?: string;
  description?: string;
}

export interface UpdateUserData {
  firstName: string;
  lastName: string;
  role: UserRole;
  isActive: boolean;
}

export interface LoginHistoryEntry {
  id: number;
  ipAddress: string;
  loginTime: string;
  logoutTime?: string;
  userAgent: string;
  isSuccessful: boolean;
}

export interface AuthContextType {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  login: (credentials: LoginCredentials) => Promise<{ success: boolean; message?: string }>;
  logout: () => Promise<void>;
  updateUser: (userData: Partial<User>) => void;
}

export interface ApiResponse<T = any> {
  data: T;
  message?: string;
}

export interface ApiError {
  message: string;
  status?: number;
}