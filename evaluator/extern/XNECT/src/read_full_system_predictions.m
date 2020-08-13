# Matlab example of how to read the 2D and 3D predictions written out by XNect
function pred = read_full_system_predictions(file_path)

%file_path = '/HPS/XNECT2/nobackup/mpii-inf/ts1/';

p2d = dlmread([file_path, 'raw2D.txt']);
p3d = dlmread([file_path, 'raw3D.txt']);
i3d = dlmread([file_path, 'IK3D.txt']);

max_idx = i3d(end,1)+1;

pred = struct([]);
for i = 1:max_idx
    for j = 1:10
        pred(i).pred2d{j} = zeros(2,14);
        pred(i).pred3d{j} = zeros(3,21);
        pred(i).ik3d{j} = zeros(3,21);
        pred(i).vis{j} = zeros(1,14);
    end
    pred(i).valid_raw = [false, false, false, false, false, false, false, false, false, false];
    pred(i).valid_ik = [false, false, false, false, false, false, false, false, false, false ];
end

for i = 1: size(p3d,1)
    idx = p2d(i,1)+1;
    pidx = p2d(i,2)+1;
    pred(idx).pred2d{pidx} = reshape(p2d(i,3:end), 2, []);
    tmp = reshape(p3d(i,3:end), 3, []);
    tmp(1:2,:) = -tmp(1:2,:);
    pred(idx).pred3d{pidx} = bsxfun(@minus, tmp, tmp(:,15));
    pred(idx).valid_raw(pidx) = true;
    pred(idx).vis{pidx} = (pred(idx).pred2d{pidx}(1,:)>0 & pred(idx).pred2d{pidx}(2,:)>0);
end
for i = 1: size(i3d,1)
    idx = i3d(i,1)+1;
    pidx = i3d(i,2)+1;
    
    tmp = reshape(i3d(i,3:end), 3, []);
    tmp(1:2,:) = -tmp(1:2,:);
    pred(idx).ik3d{pidx} = bsxfun(@minus, tmp, tmp(:,15));
    pred(idx).valid_ik(pidx) = true;
end

end
