
import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { 
  User, 
  Mail, 
  Calendar, 
  MoreVertical, 
  Plus,
  Search,
  Filter,
  CheckCircle
} from 'lucide-react';
import { InviteMemberModal } from './InviteMemberModal';
import { apiService, User as UserType } from '../services/api';
import { useToast } from '@/hooks/use-toast';

export const UserManagement = () => {
  const [users, setUsers] = useState<UserType[]>([]);
  const [loading, setLoading] = useState(true);
  const { toast } = useToast();

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      const usersData = await apiService.getUsers();
      setUsers(usersData);
    } catch (error) {
      console.error('Error fetching users:', error);
      toast({
        title: "Error",
        description: "Failed to fetch users. Please try again.",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const getRoleColor = (role: string) => {
    switch (role) {
      case 'Admin': return 'bg-red-100 text-red-700 border-red-200';
      case 'Manager': return 'bg-blue-100 text-blue-700 border-blue-200';
      case 'Developer': return 'bg-green-100 text-green-700 border-green-200';
      default: return 'bg-slate-100 text-slate-700 border-slate-200';
    }
  };

  const getInitials = (firstName: string, lastName: string) => {
    return `${firstName.charAt(0)}${lastName.charAt(0)}`;
  };

  const generateAvatarColor = (id: number) => {
    const colors = [
      'from-blue-400 to-blue-600',
      'from-green-400 to-green-600',
      'from-purple-400 to-purple-600',
      'from-pink-400 to-pink-600',
      'from-orange-400 to-orange-600',
    ];
    return colors[id % colors.length];
  };

  const handleMemberInvited = async (member: any) => {
    setUsers(prev => [...prev, member]);
  };

  if (loading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div>
            <h1 className="text-3xl font-bold text-slate-900">Team Management</h1>
            <p className="text-slate-600 mt-1">Loading team members...</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-slate-900">Team Management</h1>
          <p className="text-slate-600 mt-1">Manage team members and their roles</p>
        </div>
        <InviteMemberModal onMemberInvited={handleMemberInvited} />
      </div>

      {/* Filters */}
      <div className="flex items-center space-x-4">
        <div className="flex-1 max-w-md">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-slate-400" />
            <Input 
              placeholder="Search team members..." 
              className="pl-10 bg-white border-slate-200"
            />
          </div>
        </div>
        <Button variant="outline" className="border-slate-200">
          <Filter className="h-4 w-4 mr-2" />
          Filter
        </Button>
      </div>

      {/* Users Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {users.map((user) => (
          <Card key={user.id} className="border-0 shadow-lg hover:shadow-xl transition-all duration-300">
            <CardHeader className="text-center pb-4">
              <div className="relative mx-auto">
                <div className={`w-16 h-16 rounded-full bg-gradient-to-r ${generateAvatarColor(user.id)} flex items-center justify-center text-white text-xl font-bold`}>
                  {getInitials(user.firstName, user.lastName)}
                </div>
                {user.isActive && (
                  <div className="absolute -bottom-1 -right-1 w-6 h-6 bg-green-500 rounded-full flex items-center justify-center">
                    <CheckCircle className="h-4 w-4 text-white" />
                  </div>
                )}
              </div>
              <div className="mt-4">
                <CardTitle className="text-xl">
                  {user.firstName} {user.lastName}
                </CardTitle>
                <div className="flex items-center justify-center mt-2">
                  <Badge className={getRoleColor(user.role)}>
                    {user.role}
                  </Badge>
                </div>
              </div>
            </CardHeader>

            <CardContent className="space-y-4">
              <div className="space-y-3 text-sm">
                <div className="flex items-center space-x-2 text-slate-600">
                  <Mail className="h-4 w-4" />
                  <span className="truncate">{user.email}</span>
                </div>
                <div className="flex items-center space-x-2 text-slate-600">
                  <Calendar className="h-4 w-4" />
                  <span>Joined {new Date(user.createdAt).toLocaleDateString()}</span>
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4 pt-4 border-t border-slate-100">
                <div className="text-center">
                  <div className="text-2xl font-bold text-blue-600">0</div>
                  <div className="text-xs text-slate-500">Projects</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-green-600">0</div>
                  <div className="text-xs text-slate-500">Tasks Done</div>
                </div>
              </div>

              <div className="flex items-center justify-between pt-4">
                <div className={`text-xs px-2 py-1 rounded-full ${
                  user.isActive 
                    ? 'bg-green-100 text-green-700' 
                    : 'bg-slate-100 text-slate-700'
                }`}>
                  {user.isActive ? 'Active' : 'Inactive'}
                </div>
                <Button variant="ghost" size="sm">
                  <MoreVertical className="h-4 w-4" />
                </Button>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>

      {users.length === 0 && (
        <div className="text-center py-12">
          <p className="text-slate-500">No team members found. Invite your first team member to get started!</p>
        </div>
      )}
      
      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mt-8">
        <Card className="border-0 shadow-lg">
          <CardContent className="p-6 text-center">
            <div className="text-3xl font-bold text-blue-600 mb-2">{users.length}</div>
            <div className="text-sm text-slate-600">Total Members</div>
          </CardContent>
        </Card>
        <Card className="border-0 shadow-lg">
          <CardContent className="p-6 text-center">
            <div className="text-3xl font-bold text-green-600 mb-2">{users.filter(u => u.isActive).length}</div>
            <div className="text-sm text-slate-600">Active Members</div>
          </CardContent>
        </Card>
        <Card className="border-0 shadow-lg">
          <CardContent className="p-6 text-center">
            <div className="text-3xl font-bold text-orange-600 mb-2">0</div>
            <div className="text-sm text-slate-600">Total Tasks</div>
          </CardContent>
        </Card>
        <Card className="border-0 shadow-lg">
          <CardContent className="p-6 text-center">
            <div className="text-3xl font-bold text-purple-600 mb-2">0%</div>
            <div className="text-sm text-slate-600">Completion Rate</div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
